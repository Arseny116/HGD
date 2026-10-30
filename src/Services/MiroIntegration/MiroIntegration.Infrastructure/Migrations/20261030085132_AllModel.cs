using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiroIntegration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AllModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DesignDocumentBlocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Guidelines = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignDocumentBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DesignDocumentBlocks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlowchartBlocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowchartBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowchartBlocks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameplayBlocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ReferenceImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameplayBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameplayBlocks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MetaBlocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    GameName = table.Column<string>(type: "text", nullable: false),
                    Genre = table.Column<string>(type: "text", nullable: false),
                    GameElements = table.Column<string>(type: "text", nullable: false),
                    PlayerCount = table.Column<int>(type: "integer", nullable: false),
                    CoverImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetaBlocks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pdfs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FileUrl = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pdfs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pdfs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerBlocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReferenceImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerBlocks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalSpecsBlocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    TechnicalForm = table.Column<string>(type: "text", nullable: false),
                    View = table.Column<string>(type: "text", nullable: false),
                    Platforms = table.Column<List<string>>(type: "text[]", nullable: false),
                    Languages = table.Column<List<string>>(type: "text[]", nullable: false),
                    ReferenceImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalSpecsBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalSpecsBlocks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UiBlocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    LayoutImageUrl = table.Column<string>(type: "text", nullable: true),
                    LayoutDescription = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UiBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UiBlocks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DesignDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DesignDocumentBlockId = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DesignDefinitions_DesignDocumentBlocks_DesignDocumentBlockId",
                        column: x => x.DesignDocumentBlockId,
                        principalTable: "DesignDocumentBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlowchartEdges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FlowchartBlockId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromNodeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToNodeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Label = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowchartEdges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowchartEdges_FlowchartBlocks_FlowchartBlockId",
                        column: x => x.FlowchartBlockId,
                        principalTable: "FlowchartBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlowchartNodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FlowchartBlockId = table.Column<Guid>(type: "uuid", nullable: false),
                    Label = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    X = table.Column<double>(type: "double precision", nullable: false),
                    Y = table.Column<double>(type: "double precision", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowchartNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowchartNodes_FlowchartBlocks_FlowchartBlockId",
                        column: x => x.FlowchartBlockId,
                        principalTable: "FlowchartBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameplayOutlineItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameplayBlockId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameplayOutlineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameplayOutlineItems_GameplayBlocks_GameplayBlockId",
                        column: x => x.GameplayBlockId,
                        principalTable: "GameplayBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerBlockId = table.Column<Guid>(type: "uuid", nullable: false),
                    Health = table.Column<int>(type: "integer", nullable: false),
                    Weapons = table.Column<List<string>>(type: "text[]", nullable: false),
                    Actions = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerDefinitions_PlayerBlocks_PlayerBlockId",
                        column: x => x.PlayerBlockId,
                        principalTable: "PlayerBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerProperties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerBlockId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Feedback = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerProperties_PlayerBlocks_PlayerBlockId",
                        column: x => x.PlayerBlockId,
                        principalTable: "PlayerBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerRewards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerBlockId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Effect = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IconUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerRewards_PlayerBlocks_PlayerBlockId",
                        column: x => x.PlayerBlockId,
                        principalTable: "PlayerBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UiControls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UiBlockId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IconUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UiControls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UiControls_UiBlocks_UiBlockId",
                        column: x => x.UiBlockId,
                        principalTable: "UiBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DesignDefinitions_DesignDocumentBlockId",
                table: "DesignDefinitions",
                column: "DesignDocumentBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_DesignDocumentBlocks_ProjectId",
                table: "DesignDocumentBlocks",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlowchartBlocks_ProjectId",
                table: "FlowchartBlocks",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlowchartEdges_FlowchartBlockId",
                table: "FlowchartEdges",
                column: "FlowchartBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowchartNodes_FlowchartBlockId",
                table: "FlowchartNodes",
                column: "FlowchartBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_GameplayBlocks_ProjectId",
                table: "GameplayBlocks",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameplayOutlineItems_GameplayBlockId",
                table: "GameplayOutlineItems",
                column: "GameplayBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_MetaBlocks_ProjectId",
                table: "MetaBlocks",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pdfs_ProjectId",
                table: "Pdfs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBlocks_ProjectId",
                table: "PlayerBlocks",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDefinitions_PlayerBlockId",
                table: "PlayerDefinitions",
                column: "PlayerBlockId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerProperties_PlayerBlockId",
                table: "PlayerProperties",
                column: "PlayerBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerRewards_PlayerBlockId",
                table: "PlayerRewards",
                column: "PlayerBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserId",
                table: "Projects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSpecsBlocks_ProjectId",
                table: "TechnicalSpecsBlocks",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UiBlocks_ProjectId",
                table: "UiBlocks",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UiControls_UiBlockId",
                table: "UiControls",
                column: "UiBlockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DesignDefinitions");

            migrationBuilder.DropTable(
                name: "FlowchartEdges");

            migrationBuilder.DropTable(
                name: "FlowchartNodes");

            migrationBuilder.DropTable(
                name: "GameplayOutlineItems");

            migrationBuilder.DropTable(
                name: "MetaBlocks");

            migrationBuilder.DropTable(
                name: "Pdfs");

            migrationBuilder.DropTable(
                name: "PlayerDefinitions");

            migrationBuilder.DropTable(
                name: "PlayerProperties");

            migrationBuilder.DropTable(
                name: "PlayerRewards");

            migrationBuilder.DropTable(
                name: "TechnicalSpecsBlocks");

            migrationBuilder.DropTable(
                name: "UiControls");

            migrationBuilder.DropTable(
                name: "DesignDocumentBlocks");

            migrationBuilder.DropTable(
                name: "FlowchartBlocks");

            migrationBuilder.DropTable(
                name: "GameplayBlocks");

            migrationBuilder.DropTable(
                name: "PlayerBlocks");

            migrationBuilder.DropTable(
                name: "UiBlocks");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
