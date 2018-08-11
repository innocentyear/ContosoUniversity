using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;

namespace ContosoUniversity.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly SchoolContext _context;

        public InstructorsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Instructors
        public async Task<IActionResult> Index(int? id,int? courseID)
        {
            var viewModel = new InstructorIndexData();
            viewModel.Instructors = await _context.Instructors
                  .Include(i => i.OfficeAssignment)
                  .Include(i => i.CourseAssignments)
                     .ThenInclude(ca => ca.Course)
                         .ThenInclude(c => c.Enrollments)
                            .ThenInclude(e => e.Student)
                  .Include(i => i.CourseAssignments)
                    .ThenInclude(ca => ca.Course)
                      .ThenInclude(c => c.Department)
                  //.AsNoTracking() 如果引入代理类开户lazy loading,默认为eager loading,访问的实体必须被跟踪，不能使用AsNoTracking
                  .OrderBy(i => i.LastName)
                  .ToListAsync();

            if (id != null)
            {
                ViewData["InstructorID"] = id.Value;
                Instructor instructor = viewModel.Instructors.Where(i => i.ID == id.Value).Single();
                viewModel.Courses = instructor.CourseAssignments.Select(s => s.Course);
            }

            if (courseID != null)
            {
                ViewData["CourseID"] = courseID.Value;
                viewModel.Enrollments = viewModel.Courses.Where(x => x.CourseID == courseID).Single().Enrollments;
            }

            return View(viewModel);
            

            //return View(await _context.Instructors.ToListAsync());
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            var instructor = new Instructor();
            instructor.CourseAssignments = new List<CourseAssignment>();
            PopulateAssignedCourseData(instructor);
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LastName,FirstMidName,HireDate,OfficeAssignment")] Instructor instructor,int[] selectedCourses) //可以绑定导航属性，即便是导航属性为复杂对象。
        {
            //对于1对0或1关系，在创建1方的代码中，通过模型绑定自动完成。更是无需使用关联数据。
            //对于1对多关系，需要先初始化一个空的列表，然后，新建一个导航属性的对象，加到内存中的模型绑定的对象的导航属性的列表上。直接_context.update(T)，原实体和关联数据一起更新了。
            if (selectedCourses !=null)
            {
                instructor.CourseAssignments = new List<CourseAssignment>(); //Notice that in order to be able to add courses to the CourseAssignments navigation property you have to initialize the property as an empty collection:
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = new CourseAssignment { InstructorID = instructor.ID, CourseID = course };
                    instructor.CourseAssignments.Add(courseToAdd); //先把导航属性加到模型绑定创建的内存中的Instructor来。any course selections that were made are automatically restored.
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(instructor); //如果是直接创建，可以将Instructor和他导航属性字段一并保存到数据库。
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateAssignedCourseData(instructor);
            return View(instructor);

            /*
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            return View(instructor); */
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var instructor = await _context.Instructors.FindAsync(id);
            var instructor = await _context.Instructors
                 .Include(i => i.OfficeAssignment)
                 .Include(i =>i.CourseAssignments).ThenInclude(ca =>ca.Course) //
                 //.AsNoTracking()
                 .SingleOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            PopulateAssignedCourseData(instructor);

            return View(instructor);
        }



        // POST: Instructors/Edit/5  同样也可以使用。
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
         [ValidateAntiForgeryToken]
          public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstMidName,HireDate,OfficeAssignment")] Instructor instructor)
           {
             if (id != instructor.ID)
             {
                 return NotFound();
             }

             if (ModelState.IsValid)
             {
                 try
                 {
                    var instructorToUpdate = await _context.Instructors.Include(i =>i.OfficeAssignment).SingleOrDefaultAsync(i =>i.ID == instructor.ID);
                    instructorToUpdate.FirstMidName = instructor.FirstMidName;
                    instructorToUpdate.LastName = instructor.LastName;
                    instructorToUpdate.HireDate = instructor.HireDate;
                    if (!string.IsNullOrWhiteSpace(instructor.OfficeAssignment.Location))
                    {
                        if (instructorToUpdate.OfficeAssignment == null)
                        {
                            _context.OfficeAssignments.Add(new OfficeAssignment { InstructorID = instructorToUpdate.ID, Location = instructor.OfficeAssignment.Location });
                        }
                        instructorToUpdate.OfficeAssignment.Location = instructor.OfficeAssignment.Location;
                    }
                    else
                    {
                        instructorToUpdate.OfficeAssignment = null;
                    }

                     await _context.SaveChangesAsync();
                 }
                 catch (DbUpdateConcurrencyException)
                 {
                     if (!InstructorExists(instructor.ID))
                     {
                         return NotFound();
                     }
                     else
                     {
                         throw;
                     }
                 }
                 return RedirectToAction(nameof(Index));
             }
             return View(instructor);
         }
      */

      
        //HTTPPOST:Instructor/Edit/5  采用catch，update的方式
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,int[] selectedCourses) //模型绑定能够自动将string[]转换为int[]
        {
            //对于1对1关系，首先要预先加载关联数据，最后使用TryUpdateModel绑定导航属性，增加、更改不用处理，但要删除关联数据，需要代码来设定外键为null.
            //对于多对多关系，首先要预先加载关联数据，在读取数据库的对象上增关联数据。首先，通过检索实体的导航属性找到相关实体，在数据库上下文上删关联数据。
            if (id == null)
            {
                return NotFound();
            }

            var instructorToUpdate = await _context.Instructors 
                .Include(i => i.OfficeAssignment)
                .Include(i =>i.CourseAssignments).ThenInclude(ca =>ca.Course) //预先加载
                .SingleOrDefaultAsync(i => i.ID == id);

            if (await TryUpdateModelAsync<Instructor>( //可以简化为TryUpdateModelAsync（） 不要泛型  
                instructorToUpdate,
                "", 
                i => i.FirstMidName, i => i.LastName, i => i.HireDate, i => i.OfficeAssignment))  // TryUpdateModel 这种显式绑定功能比[Bind]隐式绑定功能强大，因为它是先读取包含导航属性的实体，然后和导航属性一起绑定。
            {
                if (String.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment.Location))
                {
                    instructorToUpdate.OfficeAssignment = null; //清空关联的OfficeAssignment; 
                }
                UpdateInstructorCourses(selectedCourses, instructorToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                  // Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }

                return RedirectToAction(nameof(Index));
            }
            PopulateAssignedCourseData(instructorToUpdate);
            return View(instructorToUpdate);
        }

     

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //对于1对0或1关系，由于设置了级联删除规则，删除了实体会自动删除0 1依赖方。
            //对于多对多关系，首先预先加载关联数据，删除了实体，也会引发级联删除，删除了实体会自动删除连接表。
            //对于1对0或多关系，然后从数据库上下文中导航属性的dbset上，依赖的ID设置为空值。
            // var instructor = await _context.Instructors.FindAsync(id);
            var instructor = await _context.Instructors
                 .Include(i => i.CourseAssignments)  //只有设置了eager loading CourseAssignment ，EF才能删除对应的CourseAssignment
                 .SingleAsync(i => i.ID == id);  //由于OfficeAssignment依赖于InstructorID ,Instructor 删除了，就自动删除了对应的Office

            var departments = await _context  //但是由于Departments中 Administrator可以为空，因此，不会引发级联删除，需要手动删除其引用。
                .Departments.Where(d => d.InstructorID == id)
                .ToListAsync();
            departments.ForEach(d => d.InstructorID = null);

            //_context.Instructors.Remove(instructor); 
            _context.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.ID == id);
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = _context.Courses;
            var instructorCourse = new HashSet<int>(instructor.CourseAssignments.Select(ca => ca.CourseID));//Select用于一个序列中的每一个元素
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourse.Contains(course.CourseID)
                });
            }

            ViewData["Courses"] = viewModel;
        }


        private void UpdateInstructorCourses(int[] selectedCourses, Instructor instructorToUpdate)
        {
            if (selectedCourses == null) //如果没有选择任何一门课程， 就清空 CourseAssignment导航属性。
            {
                instructorToUpdate.CourseAssignments = new List<CourseAssignment>();
                return;
            }

            var selectedCourseHS = new HashSet<int>(selectedCourses);
            var instructorCourses = new HashSet<int>(instructorToUpdate.CourseAssignments.Select(c => c.CourseID));

            foreach (var course in _context.Courses)
            {
                if (selectedCourseHS.Contains(course.CourseID))
                {
                    if (!instructorCourses.Contains(course.CourseID))
                    { 
                        instructorToUpdate.CourseAssignments.Add(new CourseAssignment { InstructorID = instructorToUpdate.ID, CourseID = course.CourseID });
                        //EF 6中由于为纯连接表建立了隐式的 纯连接表，语句 为 instructorToUpdate.Courses.Add(course);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.CourseID))
                    {
                        CourseAssignment courseToRemove = instructorToUpdate.CourseAssignments.SingleOrDefault(i => i.CourseID == course.CourseID);
                       // _context.CourseAssignments.Remove(courseToRemove); 
                        _context.Remove(courseToRemove);  //在.NET Core中，直接在数据库上下文中调用_context.Remove()与上面的在上下文DbSet<>上调用_context.CouseAssignments.Remove()是等价的，相同的API还有_context.Add、_context.Update()是等价的。
                       //EF 6中由于为纯连接表建立了隐式的 纯连接表，语句 为instructorToUpdate.Courses.Remove(course);


                    }
                }
            }
               

        }
    }
}
