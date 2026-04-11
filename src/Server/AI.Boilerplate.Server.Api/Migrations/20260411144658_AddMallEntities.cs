using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.Boilerplate.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMallEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主键ID"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户ID"),
                    RecipientName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "收件人姓名"),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, comment: "联系电话"),
                    Province = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "省"),
                    City = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "市"),
                    District = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "区/县"),
                    StreetAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, comment: "详细地址"),
                    PostalCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true, comment: "邮政编码"),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, comment: "是否默认地址"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "创建时间"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "更新时间"),
                    Orders = table.Column<int[]>(type: "integer[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "用户收货/账单地址表");

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主键ID"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户ID"),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false, comment: "产品ID"),
                    Quantity = table.Column<int>(type: "integer", nullable: false, comment: "数量"),
                    Selected = table.Column<bool>(type: "boolean", nullable: false, comment: "结算是否勾选"),
                    AddedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "加入购物车时间"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "购物车项表");

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主键ID"),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false, comment: "产品ID"),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false, comment: "当前可用库存"),
                    ReservedQuantity = table.Column<int>(type: "integer", nullable: false, comment: "预占库存(已下单未支付)"),
                    LowStockThreshold = table.Column<int>(type: "integer", nullable: false, comment: "低库存预警阈值"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "最后更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "产品库存表");

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主键ID"),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false, comment: "产品ID"),
                    ImageUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false, comment: "图片存储URL"),
                    AltText = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "图片替代文本"),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, comment: "排序权重(升序)"),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, comment: "是否为主图"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "创建时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "产品图片表");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主键ID"),
                    OrderNo = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "订单业务编号(唯一)"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户ID"),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false, comment: "商品原价总额"),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false, comment: "优惠抵扣金额"),
                    ShippingFee = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false, comment: "运费"),
                    PayableAmount = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false, comment: "实际应付金额"),
                    Status = table.Column<short>(type: "smallint", nullable: false, comment: "订单状态(0:待付款 1:已付款 2:已发货 3:已完成 4:已取消 5:退款中)"),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: false, comment: "收货地址ID"),
                    Remark = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "买家备注"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "创建时间"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "更新时间"),
                    PaidOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "支付成功时间"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "逻辑删除标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "订单主表");

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主键ID"),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false, comment: "订单ID"),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false, comment: "产品ID"),
                    ProductName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "下单时产品名称快照"),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false, comment: "下单时单价快照"),
                    Quantity = table.Column<int>(type: "integer", nullable: false, comment: "购买数量"),
                    SubTotal = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false, comment: "行小计金额"),
                    PrimaryImageAltText = table.Column<string>(type: "text", nullable: true, comment: "下单时主图替代文本快照")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "订单明细表（快照数据）");

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主键ID"),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false, comment: "订单ID"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户ID"),
                    Amount = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false, comment: "实际支付金额"),
                    PaymentMethod = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "支付渠道(Alipay, WeChatPay, UnionPay等)"),
                    TransactionId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "第三方支付平台流水号"),
                    Status = table.Column<short>(type: "smallint", nullable: false, comment: "支付状态(0:待支付 1:成功 2:失败 3:已退款)"),
                    PaidOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "支付成功时间"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "记录创建时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "支付流水记录表");

            migrationBuilder.CreateTable(
                name: "ProductReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主键ID"),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false, comment: "订单ID"),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false, comment: "产品ID"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户ID"),
                    Rating = table.Column<short>(type: "smallint", nullable: false, comment: "评分(1-5)"),
                    Comment = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "评价内容"),
                    IsAnonymous = table.Column<bool>(type: "boolean", nullable: false, comment: "是否匿名显示"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "评价时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductReviews_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductReviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductReviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "产品评价表");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId_IsDefault",
                table: "Addresses",
                columns: new[] { "UserId", "IsDefault" },
                unique: true,
                filter: "\"IsDefault\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserId",
                table: "CartItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserId_ProductId",
                table: "CartItems",
                columns: new[] { "UserId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductId",
                table: "Inventories",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressId",
                table: "Orders",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNo",
                table: "Orders",
                column: "OrderNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status",
                table: "Orders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Status",
                table: "Payments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TransactionId",
                table: "Payments",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId_IsPrimary",
                table: "ProductImages",
                columns: new[] { "ProductId", "IsPrimary" },
                unique: true,
                filter: "\"IsPrimary\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_OrderId_ProductId",
                table: "ProductReviews",
                columns: new[] { "OrderId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_ProductId",
                table: "ProductReviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_Rating",
                table: "ProductReviews",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_UserId",
                table: "ProductReviews",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductReviews");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
