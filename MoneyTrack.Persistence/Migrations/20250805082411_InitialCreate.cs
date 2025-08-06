using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MoneyTrack.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "api_usage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApiName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CallDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CallCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_usage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    ExpenseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transaction_transaction_category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "transaction_category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transaction_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_role", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_user_role_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_role_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "LastModifiedBy", "LastModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("4c5d1784-f350-49da-861c-92c486b4b46c"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest role", null, null, 1 },
                    { new Guid("af2b2a21-21e7-41a2-8727-c67816796132"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Administrator role", null, null, 0 }
                });

            migrationBuilder.InsertData(
                table: "transaction_category",
                columns: new[] { "Id", "Code", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("0c1a8bde-4e0a-43b2-a8e6-68ad3e2f8fb2"), "ENTERTAINMENT_001", "Giải Trí, xem phim, mua gói netflix, HBO, spotify, apple music, sách, truyện tranhTham gia các trò chơi tốn phí, cầu lông, tennis, bóng bàn, bóng rổTham gia các hoạt động giải trí như di concert, ca nhạc, karaoke, hát hò, nhảy múa, barĐi nhậu", "Entertainment" },
                    { new Guid("25949f3f-6170-4544-9683-a5a920c0d204"), "LIVING_001", "Grab, bee, now, giao hàng, Uber, xe ôm, cắt tóc, cạo râu, gội đầu, 4g, thú cưng.", "Living" },
                    { new Guid("82a956cc-037f-4866-8008-d641448c485e"), "UNFORESEEN_001", "Bất Ngờ, sửa chữa, thay thế, mua mới, mất mát, hỏng hóc, hỏng hóc, hỏng hóc, hỏng hóc, hỏng hócĐám cưới, đám tang, sinh nhật, lễ tết, lễ hội, lễ hội, lễ hội, lễ hội, lễ hộiQuà tặng, Quà sinh nhật, quà 8/3, quà 14/2", "Unforeseen" },
                    { new Guid("88d9d4e4-8a6d-42b9-9495-74bfe29c1b97"), "FOOD_001", "Ăn Uống, nhà hàng, quán ăn, quán cà phê, quán trà sữa, quán kem, quán bánh mì, quán phở, quán cơm, buffetsiêu thị, cửa hàng tiện lợi, cửa hàng thực phẩm, cửa hàng thực phẩm, cửa hàng thực phẩm, bách hóa xanh, winmartmì gói, trà, bánh kẹo, snack, dinh dưỡng", "Food" },
                    { new Guid("b3e7df94-22d7-4f8e-9f1d-9f8eb6dfc1c2"), "SHOPPING_001", "Mua Sắm, Điện tử & Công nghệ, Điện tử & Công nghệ, Điện tử & Công nghệ.", "Shopping" },
                    { new Guid("b72c7b35-cf25-4df7-930b-9ebfa237cd6a"), "EDUCATION_001", "Giáo Dục, học phí, chứng chỉ, gia sư, luyện thi, trung tâm, học nghề, sách, dụng cụ học tập, phần mềm học tập", "Education" },
                    { new Guid("c9da16f1-87ad-4ddb-924f-4ea7bea5a175"), "INVESTMENT_001", "Đầu tư, Đầu tư chứng khoán, đầu tư bất động sản, tiền ảo, bitcoin, chứng chỉ quỹ, quỹ đầu tư, trái phiếu, nuối trồng thủy sản, hải sản, xây dựng, mở quán cafe, start-up.", "Investment" },
                    { new Guid("e4d93c9e-1c0f-44c6-bb67-7fc74d3c1e6f"), "TRAVEL_001", "Du Lịch, vé máy bay, vé tàu, vé xe, khách sạn, homestay, resort, tour du lịch, vé tham quan, vé vào cổngvé vào cổng khu du lịchPhí dic vụ, phí hướng dẫn viên, phí hướng dẫn viên, phí hướng dẫn viên", "Travel" },
                    { new Guid("fa7eb29d-bbdf-4e49-a5df-f173d9c7c27e"), "FIXED_001", "Chi Phí Cố Định, tiền nhà, tiền điện, tiền nước, tiền internet, tiền mạng, phí vệ sinh, tiền gửi xe hàng thángBảo hiểm, bảo hiểm nhân thọ, trả góp tín dụng, học phí, thẻ hội viên gym, bơi, cầu lông, yoga, thể hìnhthực phẩm chức năng, thực phẩm chức năng, thực phẩm chức năngĐăng ký Netflix, Spotify, YouTube Premium", "Fixed Cost" },
                    { new Guid("fb91ef86-df65-4d66-bb2b-62e6d8c5d29d"), "HEALTH_CARE_001", "Sức Khỏe, thực phẩm chức năng, thực phẩm chức năng, thực phẩm chức năng, viện phí, khám chữa bệnh, phẫu thuật, xét nghiệm, Mua thuốc theo đơn, thuốc không kê đơnMua thuốc theo đơn, thuốc không kê đơn, Mua thuốc theo đơn, thuốc không kê đơn, Mua thuốc theo đơn, thuốc không kê đơntư vấn trị liệu tâm lý, mát xa, mát sa, bia ôm", "Health Care" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_api_usage_UserId_ApiName_CallDate",
                table: "api_usage",
                columns: new[] { "UserId", "ApiName", "CallDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transaction_CategoryId",
                table: "transaction",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_UserId",
                table: "transaction",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_RoleId",
                table: "user_role",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "api_usage");

            migrationBuilder.DropTable(
                name: "transaction");

            migrationBuilder.DropTable(
                name: "user_role");

            migrationBuilder.DropTable(
                name: "transaction_category");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
