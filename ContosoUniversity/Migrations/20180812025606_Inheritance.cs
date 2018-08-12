using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoUniversity.Migrations
{
    public partial class Inheritance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //移除依赖于Instructor的外键
            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignment_Instructor_InstructorID",
                table: "CourseAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_Instructor_InstructorID",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficeAssginment_Instructor_InstructorID",
                table: "OfficeAssginment");

           // migrationBuilder.DropForeignKey(
               // name: "FK_Enrollment_Student_StudentID",
              //  table: "Enrollment");

           // migrationBuilder.DropPrimaryKey(
             //   name: "PK_Student",
               // table: "Student");

            //修改表名，将student 表作为person表。
            migrationBuilder.RenameTable(
                name: "Student",
                newName: "Person");

            //增加列名用来存储 instructor表的数据。
            migrationBuilder.AddColumn<DateTime>(
                name: "HireDate",
                table: "Person",
                nullable: true);

            //增加一个 Discriminator列，用来区分student 还是Instructor.
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Person",
                nullable: false,
                defaultValue: "Student");

            //修改原有列，可以使他为空。
            migrationBuilder.AlterColumn<DateTime>(
                name: "EnrollmentDate",
                table: "Person",
                nullable: true,
                oldClrType: typeof(DateTime));
            //增加一列 用来存储临时列用来升级外键
            migrationBuilder.AddColumn<int>(name: "OldID", table: "Person", nullable: true);

            //Copy Exsting Instructor data into new Persontable ，当将数据拷贝到Person表的时候，会获得新的主键值。
            migrationBuilder.Sql("INSERT INTO dbo.Person(LastName,FirstName,HireDate,EnrollmentDate,Discriminator,OldID) SELECT LastName,FirstName,HireDate,null AS EnrollmentDate,'Instructor' AS Discriminator,ID AS OldID FROM dbo.Instructor");
            //Fix up existing relationship to match new PK's
            migrationBuilder.Sql("UPDATE dbo.Department SET InstructorID =(SELECT ID FROM dbo.Person WHERE OldID = Department.InstructorID AND Discriminator ='Instructor') WHERE dbo.Department.InstructorID IS NOT NULL ");
            migrationBuilder.Sql("UPDATE dbo.CourseAssignment SET InstructorID =(SELECT ID FROM dbo.Person WHERE OldID = CourseAssignment.InstructorID AND Discriminator='Instructor') ");
            migrationBuilder.Sql("UPDATE dbo.OfficeAssginment SET InstructorID =(SELECT ID FROM dbo.Person WHERE OldID = OfficeAssginment.InstructorID AND Discriminator='Instructor')");


            //删除Instructor表
            migrationBuilder.DropTable(
             name: "Instructor");
           //删除Person表的OldID 临时列
            migrationBuilder.DropColumn(name: "OldID", table: "Person");

          //  migrationBuilder.AddPrimaryKey(
            //    name: "PK_Person",
            //    table: "Person",
            //    column: "ID");
            //修复指向insturctor的外键。
            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignment_Person_InstructorID",
                table: "CourseAssignment",
                column: "InstructorID",
                principalTable: "Person",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Person_InstructorID",
                table: "Department",
                column: "InstructorID",
                principalTable: "Person",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);//不启用级联删除

         /*   migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Person_StudentID",
                table: "Enrollment",
                column: "StudentID",
                principalTable: "Person",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
           */

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeAssginment_Person_InstructorID",
                table: "OfficeAssginment",
                column: "InstructorID",
                principalTable: "Person",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignment_Person_InstructorID",
                table: "CourseAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_Person_InstructorID",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Person_StudentID",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficeAssginment_Person_InstructorID",
                table: "OfficeAssginment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Person",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "HireDate",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Person");

            migrationBuilder.RenameTable(
                name: "Person",
                newName: "Student");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EnrollmentDate",
                table: "Student",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student",
                table: "Student",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "Instructor",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    HireDate = table.Column<DateTime>(nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructor", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignment_Instructor_InstructorID",
                table: "CourseAssignment",
                column: "InstructorID",
                principalTable: "Instructor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Instructor_InstructorID",
                table: "Department",
                column: "InstructorID",
                principalTable: "Instructor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Student_StudentID",
                table: "Enrollment",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeAssginment_Instructor_InstructorID",
                table: "OfficeAssginment",
                column: "InstructorID",
                principalTable: "Instructor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
