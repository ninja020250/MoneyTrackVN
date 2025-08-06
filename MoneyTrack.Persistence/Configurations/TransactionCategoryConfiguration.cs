using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Persistence.Configurations;

public class TransactionCategoryConfiguration : IEntityTypeConfiguration<TransactionCategoryEntity>
{
    public void Configure(EntityTypeBuilder<TransactionCategoryEntity> builder)
    {
        builder.ToTable("transaction_category");
        builder.Property(trans => trans.Id).ValueGeneratedOnAdd();

        builder.HasData(
            new TransactionCategoryEntity
            {
                Id = new Guid("b3e7df94-22d7-4f8e-9f1d-9f8eb6dfc1c2"),
                Name = "Shopping",
                Code = "SHOPPING_001",
                Description = "Mua Sắm, Điện tử & Công nghệ, Điện tử & Công nghệ, Điện tử & Công nghệ."
            },
            new TransactionCategoryEntity
            {
                Id = new Guid("0c1a8bde-4e0a-43b2-a8e6-68ad3e2f8fb2"),
                Name = "Entertainment",
                Code = "ENTERTAINMENT_001",
                Description = "Giải Trí, xem phim, mua gói netflix, HBO, spotify, apple music, sách, truyện tranh" +
                              "Tham gia các trò chơi tốn phí, cầu lông, tennis, bóng bàn, bóng rổ" +
                              "Tham gia các hoạt động giải trí như di concert, ca nhạc, karaoke, hát hò, nhảy múa, bar" +
                              "Đi nhậu"
            },
            new TransactionCategoryEntity
            {
                Id = new Guid("e4d93c9e-1c0f-44c6-bb67-7fc74d3c1e6f"),
                Name = "Travel",
                Code = "TRAVEL_001",
                Description =
                    "Du Lịch, vé máy bay, vé tàu, vé xe, khách sạn, homestay, resort, tour du lịch, vé tham quan, vé vào cổng" +
                    "vé vào cổng khu du lịch" +
                    "Phí dic vụ, phí hướng dẫn viên, phí hướng dẫn viên, phí hướng dẫn viên"
            },
            new TransactionCategoryEntity
            {
                Id = new Guid("88d9d4e4-8a6d-42b9-9495-74bfe29c1b97"),
                Name = "Food",
                Code = "FOOD_001",
                Description =
                    "Ăn Uống, nhà hàng, quán ăn, quán cà phê, quán trà sữa, quán kem, quán bánh mì, quán phở, quán cơm, buffet" +
                    "siêu thị, cửa hàng tiện lợi, cửa hàng thực phẩm, cửa hàng thực phẩm, cửa hàng thực phẩm, bách hóa xanh, winmart" +
                    "mì gói, trà, bánh kẹo, snack, dinh dưỡng"
            },
            new TransactionCategoryEntity
            {
                Id = new Guid("fa7eb29d-bbdf-4e49-a5df-f173d9c7c27e"),
                Name = "Fixed Cost",
                Code = "FIXED_001",
                Description =
                    "Chi Phí Cố Định, tiền nhà, tiền điện, tiền nước, tiền internet, tiền mạng, phí vệ sinh, tiền gửi xe hàng tháng" +
                    "Bảo hiểm, bảo hiểm nhân thọ, trả góp tín dụng, học phí, thẻ hội viên gym, bơi, cầu lông, yoga, thể hình" +
                    "thực phẩm chức năng, thực phẩm chức năng, thực phẩm chức năng" +
                    "Đăng ký Netflix, Spotify, YouTube Premium"
            },
            new TransactionCategoryEntity
            {
                Id = new Guid("b72c7b35-cf25-4df7-930b-9ebfa237cd6a"),
                Name = "Education",
                Code = "EDUCATION_001",
                Description =
                    "Giáo Dục, học phí, chứng chỉ, gia sư, luyện thi, trung tâm, học nghề, sách, dụng cụ học tập, phần mềm học tập"
            },
            new TransactionCategoryEntity
            {
                Id = new Guid("fb91ef86-df65-4d66-bb2b-62e6d8c5d29d"),
                Name = "Health Care",
                Code = "HEALTH_CARE_001",
                Description =
                    "Sức Khỏe, thực phẩm chức năng, thực phẩm chức năng, thực phẩm chức năng, viện phí, khám chữa bệnh, phẫu thuật, xét nghiệm, Mua thuốc theo đơn, thuốc không kê đơn" +
                    "Mua thuốc theo đơn, thuốc không kê đơn, Mua thuốc theo đơn, thuốc không kê đơn, Mua thuốc theo đơn, thuốc không kê đơn" +
                    "tư vấn trị liệu tâm lý, mát xa, mát sa, bia ôm"
            },
            new TransactionCategoryEntity
            {
                Id = new Guid("c9da16f1-87ad-4ddb-924f-4ea7bea5a175"),
                Name = "Investment",
                Code = "INVESTMENT_001",
                Description =
                    "Đầu tư, Đầu tư chứng khoán, đầu tư bất động sản, tiền ảo, bitcoin, chứng chỉ quỹ, quỹ đầu tư, trái phiếu, nuối trồng thủy sản, hải sản, xây dựng, mở quán cafe, start-up."
            },
            new TransactionCategoryEntity
            {
                Id = new Guid("25949f3f-6170-4544-9683-a5a920c0d204"),
                Name = "Living",
                Code = "LIVING_001",
                Description =
                    "Grab, bee, now, giao hàng, Uber, xe ôm, cắt tóc, cạo râu, gội đầu, 4g, thú cưng."
            },
            new TransactionCategoryEntity
            {
                Id = new Guid("82a956cc-037f-4866-8008-d641448c485e"),
                Name = "Unforeseen",
                Code = "UNFORESEEN_001",
                Description =
                    "Bất Ngờ, sửa chữa, thay thế, mua mới, mất mát, hỏng hóc, hỏng hóc, hỏng hóc, hỏng hóc, hỏng hóc" +
                    "Đám cưới, đám tang, sinh nhật, lễ tết, lễ hội, lễ hội, lễ hội, lễ hội, lễ hội" +
                    "Quà tặng, Quà sinh nhật, quà 8/3, quà 14/2"
            }
        );
    }
}