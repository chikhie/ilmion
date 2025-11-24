using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ilmanar.Migrations
{
    /// <inheritdoc />
    public partial class AddSectionTypeAndSeedCompleteData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Sections",
                newName: "SectionTypeId");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Modules",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SectionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Label = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "CreatedAt", "DisplayOrder", "Label", "SubjectId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Algèbre", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("11111111-1111-1111-1111-111111111112"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Géométrie", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222221"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Mécanique", 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Thermodynamique", 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333331"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Chimie Organique", 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("44444444-4444-4444-4444-444444444441"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Biologie Cellulaire", 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "SectionTypes",
                columns: new[] { "Id", "Label" },
                values: new object[,]
                {
                    { 1, "Texte" },
                    { 2, "Image" },
                    { 3, "Vidéo" },
                    { 4, "Quiz" },
                    { 5, "Code" },
                    { 6, "Exercice" }
                });

            migrationBuilder.InsertData(
                table: "Chapters",
                columns: new[] { "Id", "Content", "CreatedAt", "Description", "DisplayOrder", "DurationMinutes", "ModuleId", "Title", "UpdatedAt", "VideoId", "VideoUrl" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-1111-1111-1111-111111111111"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Découvrez les bases fondamentales de l'algèbre et ses applications", 1, 20, new Guid("11111111-1111-1111-1111-111111111111"), "Introduction à l'algèbre", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "https://test-streams.mux.dev/x36xhzz/x36xhzz.m3u8" },
                    { new Guid("aaaaaaaa-1111-1111-1111-111111111112"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Apprenez à résoudre les équations linéaires", 2, 35, new Guid("11111111-1111-1111-1111-111111111111"), "Les équations du premier degré", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null },
                    { new Guid("aaaaaaaa-1111-1111-1111-111111111113"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Résolution de systèmes d'équations à plusieurs inconnues", 3, 45, new Guid("11111111-1111-1111-1111-111111111111"), "Les systèmes d'équations", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null },
                    { new Guid("bbbbbbbb-2222-2222-2222-222222222221"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Comprendre les trois lois fondamentales du mouvement", 1, 30, new Guid("22222222-2222-2222-2222-222222222221"), "Les lois de Newton", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "https://www.youtube.com/watch?v=kKKM8Y-u7ds" },
                    { new Guid("bbbbbbbb-2222-2222-2222-222222222222"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "L'énergie liée au mouvement des corps", 2, 25, new Guid("22222222-2222-2222-2222-222222222221"), "L'énergie cinétique", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null },
                    { new Guid("cccccccc-3333-3333-3333-333333333331"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Introduction aux composés organiques simples", 1, 40, new Guid("33333333-3333-3333-3333-333333333331"), "Les hydrocarbures", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null }
                });

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "Id", "ChapterId", "Content", "CreatedAt", "DisplayOrder", "SectionTypeId", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("eeeeeeee-1111-1111-1111-111111111111"), new Guid("aaaaaaaa-1111-1111-1111-111111111111"), "L'algèbre est une branche des mathématiques qui permet de résoudre des problèmes en utilisant des symboles et des lettres pour représenter des nombres et des quantités.\n\nElle est essentielle pour comprendre de nombreux concepts mathématiques avancés.", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1, "Qu'est-ce que l'algèbre ?", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("eeeeeeee-1111-1111-1111-111111111112"), new Guid("aaaaaaaa-1111-1111-1111-111111111111"), "https://test-streams.mux.dev/x36xhzz/x36xhzz.m3u8", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 3, "Vidéo d'introduction", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("eeeeeeee-1111-1111-1111-111111111113"), new Guid("aaaaaaaa-1111-1111-1111-111111111111"), "{\"questions\":[{\"id\":1,\"question\":\"Que représente le symbole 'x' en algèbre ?\",\"options\":[\"Un nombre fixe\",\"Une variable\",\"Une opération\",\"Un résultat\"],\"correctAnswer\":1},{\"id\":2,\"question\":\"Qu'est-ce qu'une équation ?\",\"options\":[\"Une addition\",\"Une égalité entre deux expressions\",\"Un nombre\",\"Une division\"],\"correctAnswer\":1}]}", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 4, "Quiz de compréhension", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("ffffffff-2222-2222-2222-222222222221"), new Guid("bbbbbbbb-2222-2222-2222-222222222221"), "Isaac Newton a formulé trois lois fondamentales qui décrivent le mouvement des corps. Ces lois sont à la base de la mécanique classique.\n\n**Première loi** : Un corps au repos reste au repos, un corps en mouvement reste en mouvement à vitesse constante, sauf si une force extérieure agit sur lui.\n\n**Deuxième loi** : F = ma (Force = masse × accélération)\n\n**Troisième loi** : À toute action correspond une réaction égale et opposée.", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1, "Introduction aux lois de Newton", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("ffffffff-2222-2222-2222-222222222222"), new Guid("bbbbbbbb-2222-2222-2222-222222222221"), "https://images.unsplash.com/photo-1635070041078-e363dbe005cb?q=80&w=800&auto=format&fit=crop", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, "Illustration des forces", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("ffffffff-2222-2222-2222-222222222223"), new Guid("bbbbbbbb-2222-2222-2222-222222222221"), "Calculez la force nécessaire pour déplacer un objet de 5 kg avec une accélération de 2 m/s².\n\nRappel : F = ma\n\nRéponse : ____ N", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 6, "Exercice pratique", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sections_SectionTypeId",
                table: "Sections",
                column: "SectionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_SectionTypes_SectionTypeId",
                table: "Sections",
                column: "SectionTypeId",
                principalTable: "SectionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_SectionTypes_SectionTypeId",
                table: "Sections");

            migrationBuilder.DropTable(
                name: "SectionTypes");

            migrationBuilder.DropIndex(
                name: "IX_Sections_SectionTypeId",
                table: "Sections");

            migrationBuilder.DeleteData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-1111-1111-1111-111111111113"));

            migrationBuilder.DeleteData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-3333-3333-3333-333333333331"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444441"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-1111-1111-1111-111111111113"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-2222-2222-2222-222222222221"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-2222-2222-2222-222222222223"));

            migrationBuilder.DeleteData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-2222-2222-2222-222222222221"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333331"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222221"));

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Modules");

            migrationBuilder.RenameColumn(
                name: "SectionTypeId",
                table: "Sections",
                newName: "Type");
        }
    }
}
