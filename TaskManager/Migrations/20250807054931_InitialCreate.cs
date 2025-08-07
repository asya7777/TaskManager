using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    tagId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    tagName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.tagId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    usrId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    nickname = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    firstName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    lastName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    email = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.usrId);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    taskId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    taskName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    taskDeadline = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    taskDescription = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    usrId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    UserusrId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.taskId);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_UserusrId",
                        column: x => x.UserusrId,
                        principalTable: "Users",
                        principalColumn: "usrId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskTag",
                columns: table => new
                {
                    TagstagId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TaskstaskId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTag", x => new { x.TagstagId, x.TaskstaskId });
                    table.ForeignKey(
                        name: "FK_TaskTag_Tags_TagstagId",
                        column: x => x.TagstagId,
                        principalTable: "Tags",
                        principalColumn: "tagId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskTag_Tasks_TaskstaskId",
                        column: x => x.TaskstaskId,
                        principalTable: "Tasks",
                        principalColumn: "taskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserusrId",
                table: "Tasks",
                column: "UserusrId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTag_TaskstaskId",
                table: "TaskTag",
                column: "TaskstaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskTag");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
