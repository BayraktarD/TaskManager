using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Data.Migrations
{
    public partial class InitDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttachmentTypes",
                columns: table => new
                {
                    IdAttachmentType = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Attachme__9A39EABC3EF21011", x => x.IdAttachmentType);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    IdDepartment = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Departme__DF1E6E4BCB87D873", x => x.IdDepartment);
                });

            migrationBuilder.CreateTable(
                name: "JobTitles",
                columns: table => new
                {
                    IdJobTitle = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tmp_ms_x__A427B021DACEA8F8", x => x.IdJobTitle);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    IdEmployee = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CanCreateTasks = table.Column<bool>(type: "bit", nullable: false),
                    CanAssignTasks = table.Column<bool>(type: "bit", nullable: false),
                    CanModifyProfiles = table.Column<bool>(type: "bit", nullable: false),
                    CanDeleteTasks = table.Column<bool>(type: "bit", nullable: false),
                    CanCreateProfiles = table.Column<bool>(type: "bit", nullable: false),
                    CanDeleteProfiles = table.Column<bool>(type: "bit", nullable: false),
                    CanModifyTasks = table.Column<bool>(type: "bit", nullable: false),
                    JobTitle = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Department = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tmp_ms_x__B7C926384DDF13EA", x => x.IdEmployee);
                    table.ForeignKey(
                        name: "FK_Users_Departments",
                        column: x => x.Department,
                        principalTable: "Departments",
                        principalColumn: "IdDepartment");
                    table.ForeignKey(
                        name: "FK_Users_JobTitles",
                        column: x => x.JobTitle,
                        principalTable: "JobTitles",
                        principalColumn: "IdJobTitle");
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    IdTask = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EditableStartDate = table.Column<bool>(type: "bit", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EditableEndDate = table.Column<bool>(type: "bit", nullable: false),
                    AssignedToId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FinishedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TaskDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    HasAttachments = table.Column<bool>(type: "bit", nullable: false),
                    SolutionDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TaskName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tmp_ms_x__9FCAD1C5DEBBCF60", x => x.IdTask);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_AssignedTo",
                        column: x => x.AssignedToId,
                        principalTable: "Employees",
                        principalColumn: "IdEmployee");
                    table.ForeignKey(
                        name: "FK_Tasks_Users_CreatedBy",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "IdEmployee");
                    table.ForeignKey(
                        name: "FK_Tasks_Users_ModifiedBy",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "IdEmployee");
                });

            migrationBuilder.CreateTable(
                name: "TaskAttachment",
                columns: table => new
                {
                    IdTask = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdAttachment = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Attachment = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    AttachmentName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tmp_ms_x__95730B08BC22CDBC", x => new { x.IdAttachment, x.IdTask });
                    table.ForeignKey(
                        name: "FK_TaskAttachment_Tasks",
                        column: x => x.IdTask,
                        principalTable: "Tasks",
                        principalColumn: "IdTask");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Department",
                table: "Employees",
                column: "Department");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobTitle",
                table: "Employees",
                column: "JobTitle");

            migrationBuilder.CreateIndex(
                name: "UniqueUserId",
                table: "Employees",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttachment_IdTask",
                table: "TaskAttachment",
                column: "IdTask");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedToId",
                table: "Tasks",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatedById",
                table: "Tasks",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ModifiedById",
                table: "Tasks",
                column: "ModifiedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentTypes");

            migrationBuilder.DropTable(
                name: "TaskAttachment");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "JobTitles");
        }
    }
}
