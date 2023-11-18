using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FabricAir.Files.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixUpNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleFileGroups_FileGroups_FileGroupId",
                table: "RoleFileGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleFileGroups_Roles_RoleId",
                table: "RoleFileGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "UserRoles",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserRoles",
                newName: "RolesId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                newName: "IX_UserRoles_UsersId");

            migrationBuilder.RenameColumn(
                name: "FileGroupId",
                table: "RoleFileGroups",
                newName: "RolesId");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "RoleFileGroups",
                newName: "FileGroupsId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleFileGroups_FileGroupId",
                table: "RoleFileGroups",
                newName: "IX_RoleFileGroups_RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_GroupId",
                table: "Files",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_Name",
                table: "Files",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileGroups_Name",
                table: "FileGroups",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_FileGroups_GroupId",
                table: "Files",
                column: "GroupId",
                principalTable: "FileGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleFileGroups_FileGroups_FileGroupsId",
                table: "RoleFileGroups",
                column: "FileGroupsId",
                principalTable: "FileGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleFileGroups_Roles_RolesId",
                table: "RoleFileGroups",
                column: "RolesId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RolesId",
                table: "UserRoles",
                column: "RolesId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UsersId",
                table: "UserRoles",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_FileGroups_GroupId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleFileGroups_FileGroups_FileGroupsId",
                table: "RoleFileGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleFileGroups_Roles_RolesId",
                table: "RoleFileGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RolesId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UsersId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Files_GroupId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_Name",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_FileGroups_Name",
                table: "FileGroups");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "UserRoles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "RolesId",
                table: "UserRoles",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_UsersId",
                table: "UserRoles",
                newName: "IX_UserRoles_RoleId");

            migrationBuilder.RenameColumn(
                name: "RolesId",
                table: "RoleFileGroups",
                newName: "FileGroupId");

            migrationBuilder.RenameColumn(
                name: "FileGroupsId",
                table: "RoleFileGroups",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleFileGroups_RolesId",
                table: "RoleFileGroups",
                newName: "IX_RoleFileGroups_FileGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleFileGroups_FileGroups_FileGroupId",
                table: "RoleFileGroups",
                column: "FileGroupId",
                principalTable: "FileGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleFileGroups_Roles_RoleId",
                table: "RoleFileGroups",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
