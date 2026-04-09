using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.Boilerplate.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductConfigurationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "产品短编号");

            migrationBuilder.AlterColumn<int>(
                name: "ShortId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"产品短编号\"')",
                comment: "短编号",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "nextval('\"ProductShortId\"')",
                oldComment: "短编号");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionHTML",
                table: "Products",
                type: "character varying(4096)",
                maxLength: 4096,
                nullable: true,
                comment: "HTML 描述",
                oldClrType: typeof(string),
                oldType: "character varying(4096)",
                oldMaxLength: 4096,
                oldNullable: true,
                oldComment: "HTML描述");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Products",
                type: "uuid",
                nullable: false,
                comment: "类别 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "类别ID");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("10e9f8e7-b6a5-d4c3-b2a1-d0e9f8e7b6a5"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>提升驾驶性能。</h3><p>生于赛道，AMG GT Coupe 体现了纯粹跑车的本质，拥有惊人的动力和精准的操控。</p>", "提升驾驶性能。\n生于赛道，AMG GT Coupe 体现了纯粹跑车的本质，拥有惊人的动力和精准的操控。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("21d0e9f8-e7b6-a5d4-c3b2-a1d0e9f8e7b6"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>运动优雅，重新定义。</h3><p>全新 CLE Coupe 将富有表现力的设计与动态操控和先进技术相结合，创造出现代的欲望图标。</p>", "运动优雅，重新定义。\n全新 CLE Coupe 将富有表现力的设计与动态操控和先进技术相结合，创造出现代的欲望图标。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("32a1d0e9-f8e7-b6a5-d4c3-b2a1d0e9f8e7"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>从每个角度都无法抗拒。</h3><p>凭借标志性的倾斜车顶线和运动姿态，CLA Coupe 以富有表现力的设计和灵活的性能吸引着人们的目光。</p>", "从每个角度都无法抗拒。\n凭借标志性的倾斜车顶线和运动姿态，CLA Coupe 以富有表现力的设计和灵活的性能吸引着人们的目光。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("43b2a1d0-e9f8-e7b6-a5d4-c3b2a1d0e9f8"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>汽车欲望的巅峰。</h3><p>无懈可击的工程技术，S-Class Sedan 在安全、舒适和驾驶体验方面开创了创新，定义了奢华旅行。</p>", "汽车欲望的巅峰。\n无懈可击的工程技术，S-Class Sedan 在安全、舒适和驾驶体验方面开创了创新，定义了奢华旅行。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("512eb70b-1d39-4845-88c0-fe19cd2d1979"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>先进、适应性强、爱冒险。</h3><p>不仅仅是新，它可以在未来更新其先进性。不仅仅是贴心，它可以预测需求和愿望。超越未来，它可以更好地利用您的时间，并使您使用它的时间更美好。</p>", "先进、适应性强、爱冒险。\n不仅仅是新，它可以在未来更新其先进性。不仅仅是贴心，它可以预测需求和愿望。超越未来，它可以更好地利用您的时间，并使您使用它的时间更美好。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("54c3b2a1-d0e9-f8e7-b6a5-d4c3b2a1d0e9"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>品牌的核心，智能精炼。</h3><p>作为智能的杰作，E-Class Sedan 无缝融合了动感设计、奢华舒适和开创性的驾驶辅助系统。</p>", "品牌的核心，智能精炼。\n作为智能的杰作，E-Class Sedan 无缝融合了动感设计、奢华舒适和开创性的驾驶辅助系统。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("5746ae3d-5116-4774-9d55-0ff496e5186f"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>电动、必需、典范</h3><p>它是未来主义、前卫和新鲜的，但您知道它的核心价值观。不断完善的奢华。不断进步的创新。以及对您福祉的永恒奉献。也许没有哪款电动轿车能如此新颖而自然。</p>", "电动、必需、典范\n它是未来主义、前卫和新鲜的，但您知道它的核心价值观。不断完善的奢华。不断进步的创新。以及对您福祉的永恒奉献。也许没有哪款电动轿车能如此新颖而自然。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("65d4c3b2-a1d0-e9f8-e7b6-a5d4c3b2a1d0"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>一个被重新工程的图标。</h3><p>一眼就能认出，永远充满能力。G-Class 仍然是定义奢华越野车的标杆，将永恒的设计与现代科技和强悍的性能相结合。</p>", "一个被重新工程的图标。\n一眼就能认出，永远充满能力。G-Class 仍然是定义奢华越野车的标杆，将永恒的设计与现代科技和强悍的性能相结合。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("76a5d4c3-b2a1-d0e9-f8e7-b6a5d4c3b2a1"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>豪华 SUV 的 S 级。</h3><p>为多达七名乘客提供头等舱般的旅行体验，GLS 将强大的气场与无与伦比的奢华、空间和科技相结合。</p>", "豪华 SUV 的 S 级。\n为多达七名乘客提供头等舱般的旅行体验，GLS 将强大的气场与无与伦比的奢华、空间和科技相结合。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("87b6a5d4-c3b2-a1d0-e9f8-e7b6a5d4c3b2"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>运动气息，强劲性能。</h3><p>GLE Coupe 将 SUV 的肌肉感与轿跑的优雅线条相结合，带来令人振奋的性能和不容忽视的风格。</p>", "运动气息，强劲性能。\nGLE Coupe 将 SUV 的肌肉感与轿跑的优雅线条相结合，带来令人振奋的性能和不容忽视的风格。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("98e7b6a5-d4c3-b2a1-d0e9-f8e7b6a5d4c3"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>每一种地形的主宰。</h3><p>智能、宽敞且强大的 GLE SUV 为家庭和冒险者提供尖端技术和奢华舒适。</p>", "每一种地形的主宰。\n智能、宽敞且强大的 GLE SUV 为家庭和冒险者提供尖端技术和奢华舒适。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("9a59dda2-7b12-4cc1-9658-d2586eef91d7"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>探索的范围。可容纳多达 7 人。</h3><p>这是一款多功能 SUV，可容纳多达七人。是一款您可以每天享受的先进电动车。智能技术和贴心的奢华以快速响应和静音精致呈现。</p>", "探索的范围。可容纳多达 7 人。\n这是一款多功能 SUV，可容纳多达七人。是一款您可以每天享受的先进电动车。智能技术和贴心的奢华以快速响应和静音精致呈现。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-1234-567890abcdef"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>福特铸造的坚韧。</h3><p>美国最畅销的卡车，以其工作或娱乐的能力、创新和坚韧而闻名。</p>", "福特铸造的坚韧。\n美国最畅销的卡车，以其工作或娱乐的能力、创新和坚韧而闻名。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-a7b8-9012-3456c7d8e9f0"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>激进的电动皮卡。</h3><p>一款具有外骨骼设计的未来派皮卡，提供多功能性、性能和耐用性。</p>", "激进的电动皮卡。\n一款具有外骨骼设计的未来派皮卡，提供多功能性、性能和耐用性。\n", "特斯拉 Cybertruck" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a3b4c5d6-e7f8-a9b0-1234-5678c9d0e1f2"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>合适尺寸的皮卡。</h3><p>一款坚固耐用的中型卡车，旨在满足工作需求和周末冒险。</p>", "合适尺寸的皮卡。\n一款坚固耐用的中型卡车，旨在满足工作需求和周末冒险。\n", "日产 Frontier" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a5b6c7d8-e9f0-a1b2-3456-7890c1d2e3f4"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>大众市场电动轿车。</h3><p>特斯拉最实惠的车型，提供令人印象深刻的续航里程、性能和极简设计。</p>", "大众市场电动轿车。\n特斯拉最实惠的车型，提供令人印象深刻的续航里程、性能和极简设计。\n", "特斯拉 Model 3" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a7b8c9d0-e1f2-a3b4-5678-9012c3d4e5f6"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>紧凑型卡车，大构想。</h3><p>一款经济实惠且多功能的紧凑型皮卡，标配混合动力系统。</p>", "紧凑型卡车，大构想。\n一款经济实惠且多功能的紧凑型皮卡，标配混合动力系统。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a9b0c1d2-e3f4-a5b6-7890-1234c5d6e7f8"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>老板。</h3><p>原始的运动型活动车，在其细分市场中设定了奢华、性能和能力的基准。</p>", "老板。\n原始的运动型活动车，在其细分市场中设定了奢华、性能和能力的基准。\n", "宝马 X5 SAV" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a9f8e7b6-a5d4-c3b2-a1d0-e9f8e7b6a5d4"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>运动风格，SUV 实力。</h3><p>将轿跑的动感与 SUV 的多功能性相结合，GLC Coupe 在任何道路上都能展现强大的气场。</p>", "运动风格，SUV 实力。\n将轿跑的动感与 SUV 的多功能性相结合，GLC Coupe 在任何道路上都能展现强大的气场。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b0a9f8e7-b6a5-d4c3-b2a1-d0e9f8e7b6a5"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>智能驱动，令人印象深刻的设计。</h3><p>GLC SUV 在中型豪华车细分市场中为舒适性、技术和性能设定了基准，能够轻松适应您的驾驶需求。</p>", "智能驱动，令人印象深刻的设计。\nGLC SUV 在中型豪华车细分市场中为舒适性、技术和性能设定了基准，能够轻松适应您的驾驶需求。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b0c1d2e3-f4a5-b6c7-8901-2345d6e7f8a9"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>行政级运动感。</h3><p>将动态性能、尖端科技和奢华舒适完美融合，为行政级人士提供精致体验。</p>", "行政级运动感。\n将动态性能、尖端科技和奢华舒适完美融合，为行政级人士提供精致体验。\n", "宝马 5 系列轿车" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-89b0-12c3-45d678e9f0a1"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>标志性的美式肌肉车。</h3><p>激动人心的性能和无可否认的风格定义了传奇的福特野马轿跑。</p>", "标志性的美式肌肉车。\n激动人心的性能和无可否认的风格定义了传奇的福特野马轿跑。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b4c5d6e7-f8a9-b0c1-2345-6789d0e1f2a3"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>富有表现力的小型跨界车。</h3><p>通过可定制的风格脱颖而出，享受适合城市的灵活性和智能科技。</p>", "富有表现力的小型跨界车。\n通过可定制的风格脱颖而出，享受适合城市的灵活性和智能科技。\n", "日产 Kicks" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b6c7d8e9-f0a1-b2c3-4567-8901d2e3f4a5"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>紧凑型电动 SUV。</h3><p>一款多功能电动 SUV，提供充足的空间、性能和访问特斯拉超级充电网络的能力。</p>", "紧凑型电动 SUV。\n一款多功能电动 SUV，提供充足的空间、性能和访问特斯拉超级充电网络的能力。\n", "特斯拉 Model Y" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b8c9d0e1-f2a3-b4c5-6789-0123d4e5f6a7"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>时尚的中型跨界车。</h3><p>将精致的设计、智能科技和动感性能结合在一起，形成一款两厢跨界车。</p>", "时尚的中型跨界车。\n将精致的设计、智能科技和动感性能结合在一起，形成一款两厢跨界车。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c1b0a9f8-e7b6-a5d4-c3b2-a1d0e9f8e7b6"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>多功能性与空间的完美结合。</h3><p>GLB 尺寸虽小，但提供了意想不到的宽敞空间，选配第三排座椅，使其成为家庭出游的灵活且友好的紧凑型 SUV。</p>", "多功能性与空间的完美结合。\nGLB 尺寸虽小，但提供了意想不到的宽敞空间，选配第三排座椅，使其成为家庭出游的灵活且友好的紧凑型 SUV。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-c7d8-9012-3456e7f8a9b0"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>电动性能轿跑。</h3><p>宝马首款全电动 Gran Coupe，提供令人印象深刻的续航里程和标志性的宝马驾驶动态。</p>", "电动性能轿跑。\n宝马首款全电动 Gran Coupe，提供令人印象深刻的续航里程和标志性的宝马驾驶动态。\n", "宝马 i4 Gran Coupe" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-9c01-23d4-56e789f0a1b2"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>冒险准备好的 SUV。</h3><p>一款宽敞且功能强大的 SUV，专为家庭冒险而设计，提供三排座椅和现代科技。</p>", "冒险准备好的 SUV。\n一款宽敞且功能强大的 SUV，专为家庭冒险而设计，提供三排座椅和现代科技。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c5d6e7f8-a9b0-c1d2-3456-7890e1f2a3b4"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>日产的电动跨界车。</h3><p>体验未来驾驶，尽在全电动 Ariya 中，融合流线型设计和先进的电动汽车技术。</p>", "日产的电动跨界车。\n体验未来驾驶，尽在全电动 Ariya 中，融合流线型设计和先进的电动汽车技术。\n", "日产 Ariya" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c7d8e9f0-a1b2-c3d4-5678-9012e3f4a5b6"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>奢华电动基准。</h3><p>重新定义电动性能的轿车，提供令人难以置信的加速、续航和科技。</p>", "奢华电动基准。\n重新定义电动性能的轿车，提供令人难以置信的加速、续航和科技。\n", "特斯拉 Model S" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c9d0e1f2-a3b4-c5d6-7890-1234e5f6a7b8"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>适合家庭的跨界车。</h3><p>日产畅销的紧凑型 SUV，提供先进的安全功能和舒适多变的内饰。</p>", "适合家庭的跨界车。\n日产畅销的紧凑型 SUV，提供先进的安全功能和舒适多变的内饰。\n", "日产 Rogue" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d0e1f2a3-b4c5-d6e7-8901-2345f6a7b8c9"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>智能中型轿车。</h3><p>一款时尚的轿车，配备可选的全轮驱动和驾驶辅助技术。</p>", "智能中型轿车。\n一款时尚的轿车，配备可选的全轮驱动和驾驶辅助技术。\n", "日产 Altima" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d2a1b0c9-f8e7-b6a5-d4c3-b2a1d0e9f8e7"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>紧凑的车身，宏大的抱负。</h3><p>灵活而富有冒险精神，GLA 将 SUV 的多功能性与紧凑型的高效完美结合，非常适合在城市街道或风景优美的路线中穿行。</p>", "紧凑的车身，宏大的抱负。\n灵活而富有冒险精神，GLA 将 SUV 的多功能性与紧凑型的高效完美结合，非常适合在城市街道或风景优美的路线中穿行。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d2e3f4a5-b6c7-d8e9-0123-4567f8a9b0c1"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>电动科技旗舰。</h3><p>未来 SAV 的大胆愿景，具有可持续奢华、突破性科技和令人振奋的电动动力。</p>", "电动科技旗舰。\n未来 SAV 的大胆愿景，具有可持续奢华、突破性科技和令人振奋的电动动力。\n", "宝马 iX SAV" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-d0e1-f234-567890a1b2c3"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>紧凑型多功能性。</h3><p>一款时尚且高效的紧凑型 SUV，提供城市驾驶和周末度假的灵活性。</p>", "紧凑型多功能性。\n一款时尚且高效的紧凑型 SUV，提供城市驾驶和周末度假的灵活性。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d6e7f8a9-b0c1-d2e3-4567-8901f2a3b4c5"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>传奇性能。</h3><p>标志性的跑车回归，结合了永恒的设计和令人兴奋的双涡轮增压 V6 动力。</p>", "传奇性能。\n标志性的跑车回归，结合了永恒的设计和令人兴奋的双涡轮增压 V6 动力。\n", "日产 Z" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d8e9f0a1-b2c3-d4e5-6789-0123f4a5b6c7"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>带有鹰翼门的电动 SUV。</h3><p>一款独特的家庭 SUV，具有独特的鹰翼门、令人印象深刻的续航和性能。</p>", "带有鹰翼门的电动 SUV。\n一款独特的家庭 SUV，具有独特的鹰翼门、令人印象深刻的续航和性能。\n", "特斯拉 Model X" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e1f2a3b4-c5d6-e7f8-9012-3456a7b8c9d0"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>锋利的紧凑型轿车。</h3><p>在紧凑轿车级别中提供意想不到的风格、标准安全功能和优质感受。</p>", "锋利的紧凑型轿车。\n在紧凑轿车级别中提供意想不到的风格、标准安全功能和优质感受。\n", "日产 Sentra" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e3b2a1d0-c9f8-e7b6-a5d4-c3b2a1d0e9f8"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>奢华的未来，电动化。</h3><p>旗舰电动轿车将前卫设计与开创性技术和惊人性能相融合，为电动出行设定了新的标准。</p>", "奢华的未来，电动化。\n旗舰电动轿车将前卫设计与开创性技术和惊人性能相融合，为电动出行设定了新的标准。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e3f4a5b6-c7d8-e9f0-1234-5678a9b0c1d2"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>高性能图标。</h3><p>传奇的 M3 提供无与伦比的赛道准备性能，同时兼顾日常实用性。</p>", "高性能图标。\n传奇的 M3 提供无与伦比的赛道准备性能，同时兼顾日常实用性。\n", "宝马 M3 Sedan" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-e1f2-3456-7890a1b2c3d4"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>野性重生。</h3><p>图标的回归。福特 Bronco 专为强悍的越野能力和冒险而生。</p>", "野性重生。\n图标的回归。福特 Bronco 专为强悍的越野能力和冒险而生。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e7f8a9b0-c1d2-e3f4-5678-9012a3b4c5d6"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>终极驾驶机器。</h3><p>典型的运动轿车，平衡了动态性能、日常实用性和奢华感。</p>", "终极驾驶机器。\n典型的运动轿车，平衡了动态性能、日常实用性和奢华感。\n", "宝马 3 系列轿车" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e9f0a1b2-c3d4-e5f6-7890-1234a5b6c7d8"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>增强版电动运动轿车。</h3><p>在 Model 3 的基础上，增加了更快的加速、赛道模式和更运动的调校。</p>", "增强版电动运动轿车。\n在 Model 3 的基础上，增加了更快的加速、赛道模式和更运动的调校。\n", "特斯拉 Model 3 Performance" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f0a1b2c3-d4e5-f6a7-8901-2345b6c7d8e9"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>超越疯狂的速度。</h3><p>提供惊人的加速数据，使其成为有史以来最快的量产车之一。</p>", "超越疯狂的速度。\n提供惊人的加速数据，使其成为有史以来最快的量产车之一。\n", "特斯拉 Model S Plaid" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f2a3b4c5-d6e7-f8a9-0123-4567b8c9d0e1"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>坚固的 3 排 SUV。</h3><p>重返坚固。Pathfinder 提供多达八个座位和强大的性能，适合家庭冒险。</p>", "坚固的 3 排 SUV。\n重返坚固。Pathfinder 提供多达八个座位和强大的性能，适合家庭冒险。\n", "日产 Pathfinder" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f4a3b2c1-d0e9-f8a7-b6c5-d4e3f2a1b0c9"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>电动奢华 SUV 的巅峰。</h3><p>体验旗舰级舒适性和突破性技术，尽在全电动 SUV 形态中，重新定义可持续奢华，最多可容纳七人。</p>", "电动奢华 SUV 的巅峰。\n体验旗舰级舒适性和突破性技术，尽在全电动 SUV 形态中，重新定义可持续奢华，最多可容纳七人。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f4a5b6c7-d8e9-f0a1-2345-6789b0c1d2e3"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>敞篷自由。</h3><p>经典跑车体验，结合现代宝马性能、灵活性和风格。</p>", "敞篷自由。\n经典跑车体验，结合现代宝马性能、灵活性和风格。\n", "宝马 Z4 跑车" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-f2a3-4567-8901b2c3d4e5"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>电动惊喜。</h3><p>一款全电动 SUV，承载着野马的名字，带来令人振奋的性能和先进的科技。</p>", "电动惊喜。\n一款全电动 SUV，承载着野马的名字，带来令人振奋的性能和先进的科技。\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f8a9b0c1-d2e3-f4a5-6789-0123b4c5d6e7"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>多才多艺的运动型活动车。</h3><p>将动态的宝马驾驶特性与 SAV 的多功能性和实用性相结合。</p>", "多才多艺的运动型活动车。\n将动态的宝马驾驶特性与 SAV 的多功能性和实用性相结合。\n", "宝马 X3 SAV" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f9f8e7b6-a5d4-c3b2-a1d0-e9f8e7b6a5d4"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>开放式驾驶的自由，优雅的设计。</h3><p>体验敞篷驾驶的乐趣，CLE Cabriolet 提供精致的风格、四季舒适和令人振奋的性能。</p>", "开放式驾驶的自由，优雅的设计。\n体验敞篷驾驶的乐趣，CLE Cabriolet 提供精致的风格、四季舒适和令人振奋的性能。\n" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "产品短编号");

            migrationBuilder.AlterColumn<int>(
                name: "ShortId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"ProductShortId\"')",
                comment: "短编号",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "nextval('\"产品短编号\"')",
                oldComment: "短编号");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionHTML",
                table: "Products",
                type: "character varying(4096)",
                maxLength: 4096,
                nullable: true,
                comment: "HTML描述",
                oldClrType: typeof(string),
                oldType: "character varying(4096)",
                oldMaxLength: 4096,
                oldNullable: true,
                oldComment: "HTML 描述");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Products",
                type: "uuid",
                nullable: false,
                comment: "类别ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "类别 ID");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("10e9f8e7-b6a5-d4c3-b2a1-d0e9f8e7b6a5"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Driving performance, elevated.</h3><p>Born on the racetrack, the AMG GT Coupe embodies the essence of a pure sports car with breathtaking power and precision handling.</p>", "Driving performance, elevated.\nBorn on the racetrack, the AMG GT Coupe embodies the essence of a pure sports car with breathtaking power and precision handling.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("21d0e9f8-e7b6-a5d4-c3b2-a1d0e9f8e7b6"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Sporty elegance, redefined.</h3><p>The all-new CLE Coupe merges expressive design with dynamic handling and advanced technology, creating a modern icon of desire.</p>", "Sporty elegance, redefined.\nThe all-new CLE Coupe merges expressive design with dynamic handling and advanced technology, creating a modern icon of desire.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("32a1d0e9-f8e7-b6a5-d4c3-b2a1d0e9f8e7"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Irresistible from every angle.</h3><p>With its iconic sloping roofline and sporty stance, the CLA Coupe captivates with expressive design and agile performance.</p>", "Irresistible from every angle.\nWith its iconic sloping roofline and sporty stance, the CLA Coupe captivates with expressive design and agile performance.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("43b2a1d0-e9f8-e7b6-a5d4-c3b2a1d0e9f8"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>The pinnacle of automotive desire.</h3><p>Engineered without compromise, the S-Class Sedan pioneers innovations in safety, comfort, and driving experience, defining luxury travel.</p>", "The pinnacle of automotive desire.\nEngineered without compromise, the S-Class Sedan pioneers innovations in safety, comfort, and driving experience, defining luxury travel.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("512eb70b-1d39-4845-88c0-fe19cd2d1979"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Advanced, adaptive, adventurous.</h3><p>More than new, it can update its advancements down the road. More than thoughtful, it can anticipate needs and desires. Beyond futuristic, it can make better use of your time, and make your time using it better.</p>", "Advanced, adaptive, adventurous.\nMore than new, it can update its advancements down the road. More than thoughtful, it can anticipate needs and desires. Beyond futuristic, it can make better use of your time, and make your time using it better.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("54c3b2a1-d0e9-f8e7-b6a5-d4c3b2a1d0e9"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>The heart of the brand, intelligently refined.</h3><p>A masterpiece of intelligence, the E-Class Sedan seamlessly blends dynamic design, luxurious comfort, and groundbreaking driver assistance systems.</p>", "The heart of the brand, intelligently refined.\nA masterpiece of intelligence, the E-Class Sedan seamlessly blends dynamic design, luxurious comfort, and groundbreaking driver assistance systems.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("5746ae3d-5116-4774-9d55-0ff496e5186f"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Electric, essential, quintessential</h3><p>It's futuristic, forward and fresh, but you know its core values. Ever-refined luxury. Ever-advancing innovation. And a never-ending devotion to your well-being. Perhaps no electric sedan feels so new, yet so natural.</p>", "Electric, essential, quintessential\nIt's futuristic, forward and fresh, but you know its core values. Ever-refined luxury. Ever-advancing innovation. And a never-ending devotion to your well-being. Perhaps no electric sedan feels so new, yet so natural.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("65d4c3b2-a1d0-e9f8-e7b6-a5d4c3b2a1d0"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>An icon reengineered.</h3><p>Instantly recognizable, eternally capable. The G-Class remains the definitive luxury off-roader, blending timeless design with modern technology and rugged performance.</p>", "An icon reengineered.\nInstantly recognizable, eternally capable. The G-Class remains the definitive luxury off-roader, blending timeless design with modern technology and rugged performance.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("76a5d4c3-b2a1-d0e9-f8e7-b6a5d4c3b2a1"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>The S-Class of SUVs.</h3><p>Offering first-class travel for up to seven passengers, the GLS combines commanding presence with unparalleled luxury, space, and technology.</p>", "The S-Class of SUVs.\nOffering first-class travel for up to seven passengers, the GLS combines commanding presence with unparalleled luxury, space, and technology.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("87b6a5d4-c3b2-a1d0-e9f8-e7b6a5d4c3b2"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Athletic presence, commanding performance.</h3><p>The GLE Coupe blends the muscular stance of an SUV with the elegant lines of a coupe, delivering exhilarating performance and undeniable style.</p>", "Athletic presence, commanding performance.\nThe GLE Coupe blends the muscular stance of an SUV with the elegant lines of a coupe, delivering exhilarating performance and undeniable style.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("98e7b6a5-d4c3-b2a1-d0e9-f8e7b6a5d4c3"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Master of every ground.</h3><p>Intelligent, spacious, and capable, the GLE SUV offers cutting-edge technology and luxurious comfort for families and adventurers alike.</p>", "Master of every ground.\nIntelligent, spacious, and capable, the GLE SUV offers cutting-edge technology and luxurious comfort for families and adventurers alike.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("9a59dda2-7b12-4cc1-9658-d2586eef91d7"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Range to roam. Room for up to 7.</h3><p>It's a versatile SUV with room for up to seven. And an advanced EV you can enjoy every day. Intelligent technology and thoughtful luxury are delivered with swift response and silenced refinement.</p>", "Range to roam. Room for up to 7.\nIt's a versatile SUV with room for up to seven. And an advanced EV you can enjoy every day. Intelligent technology and thoughtful luxury are delivered with swift response and silenced refinement.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-1234-567890abcdef"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Built Ford Tough.</h3><p>America's best-selling truck, known for its capability, innovation, and toughness for work or play.</p>", "Built Ford Tough.\nAmerica's best-selling truck, known for its capability, innovation, and toughness for work or play.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-a7b8-9012-3456c7d8e9f0"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Radical Electric Pickup.</h3><p>A futuristic pickup truck with an exoskeleton design, offering utility, performance, and durability.</p>", "Radical Electric Pickup.\nA futuristic pickup truck with an exoskeleton design, offering utility, performance, and durability.\n", "Tesla Cybertruck" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a3b4c5d6-e7f8-a9b0-1234-5678c9d0e1f2"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Right-Sized Pickup.</h3><p>A capable and durable mid-size truck designed for both work duties and weekend adventures.</p>", "Right-Sized Pickup.\nA capable and durable mid-size truck designed for both work duties and weekend adventures.\n", "Nissan Frontier" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a5b6c7d8-e9f0-a1b2-3456-7890c1d2e3f4"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Mass-Market Electric Sedan.</h3><p>Tesla's most affordable car, offering impressive range, performance, and minimalist design.</p>", "Mass-Market Electric Sedan.\nTesla's most affordable car, offering impressive range, performance, and minimalist design.\n", "Tesla Model 3" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a7b8c9d0-e1f2-a3b4-5678-9012c3d4e5f6"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Compact Truck, Big Ideas.</h3><p>An affordable and versatile compact pickup, available with a standard hybrid powertrain.</p>", "Compact Truck, Big Ideas.\nAn affordable and versatile compact pickup, available with a standard hybrid powertrain.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a9b0c1d2-e3f4-a5b6-7890-1234c5d6e7f8"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>The Boss.</h3><p>The original Sports Activity Vehicle, setting benchmarks for luxury, performance, and capability in its class.</p>", "The Boss.\nThe original Sports Activity Vehicle, setting benchmarks for luxury, performance, and capability in its class.\n", "BMW X5 SAV" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a9f8e7b6-a5d4-c3b2-a1d0-e9f8e7b6a5d4"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Sporty style, SUV substance.</h3><p>Combining the dynamic presence of a coupe with the versatility of an SUV, the GLC Coupe makes a powerful statement on any road.</p>", "Sporty style, SUV substance.\nCombining the dynamic presence of a coupe with the versatility of an SUV, the GLC Coupe makes a powerful statement on any road.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b0a9f8e7-b6a5-d4c3-b2a1-d0e9f8e7b6a5"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Intelligent drive, impressive design.</h3><p>The GLC SUV sets benchmarks for comfort, technology, and performance in the mid-size luxury segment, adapting effortlessly to your driving needs.</p>", "Intelligent drive, impressive design.\nThe GLC SUV sets benchmarks for comfort, technology, and performance in the mid-size luxury segment, adapting effortlessly to your driving needs.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b0c1d2e3-f4a5-b6c7-8901-2345d6e7f8a9"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Executive Athleticism.</h3><p>A sophisticated blend of dynamic performance, cutting-edge technology, and luxurious comfort for the executive class.</p>", "Executive Athleticism.\nA sophisticated blend of dynamic performance, cutting-edge technology, and luxurious comfort for the executive class.\n", "BMW 5 Series Sedan" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-89b0-12c3-45d678e9f0a1"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Iconic American Muscle.</h3><p>Thrilling performance and unmistakable style define the legendary Ford Mustang coupe.</p>", "Iconic American Muscle.\nThrilling performance and unmistakable style define the legendary Ford Mustang coupe.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b4c5d6e7-f8a9-b0c1-2345-6789d0e1f2a3"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Expressive Subcompact Crossover.</h3><p>Stand out with customizable style and enjoy city-friendly agility and smart technology.</p>", "Expressive Subcompact Crossover.\nStand out with customizable style and enjoy city-friendly agility and smart technology.\n", "Nissan Kicks" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b6c7d8e9-f0a1-b2c3-4567-8901d2e3f4a5"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Compact Electric SUV.</h3><p>A versatile electric SUV offering ample space, performance, and access to Tesla's Supercharger network.</p>", "Compact Electric SUV.\nA versatile electric SUV offering ample space, performance, and access to Tesla's Supercharger network.\n", "Tesla Model Y" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b8c9d0e1-f2a3-b4c5-6789-0123d4e5f6a7"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Stylish Mid-Size Crossover.</h3><p>Combines sophisticated design with smart technology and engaging performance in a two-row crossover.</p>", "Stylish Mid-Size Crossover.\nCombines sophisticated design with smart technology and engaging performance in a two-row crossover.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c1b0a9f8-e7b6-a5d4-c3b2-a1d0e9f8e7b6"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Versatility meets space.</h3><p>Surprisingly spacious for its size, the GLB offers an optional third row, making it the flexible and family-friendly compact SUV for all your adventures.</p>", "Versatility meets space.\nSurprisingly spacious for its size, the GLB offers an optional third row, making it the flexible and family-friendly compact SUV for all your adventures.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-c7d8-9012-3456e7f8a9b0"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Electric Performance Coupe.</h3><p>BMW's first all-electric Gran Coupe, delivering impressive range and signature BMW driving dynamics.</p>", "Electric Performance Coupe.\nBMW's first all-electric Gran Coupe, delivering impressive range and signature BMW driving dynamics.\n", "BMW i4 Gran Coupe" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-9c01-23d4-56e789f0a1b2"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Adventure Ready SUV.</h3><p>A spacious and capable SUV designed for family adventures, offering three rows of seating and modern tech.</p>", "Adventure Ready SUV.\nA spacious and capable SUV designed for family adventures, offering three rows of seating and modern tech.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c5d6e7f8-a9b0-c1d2-3456-7890e1f2a3b4"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Nissan's Electric Crossover.</h3><p>Experience the future of driving with the all-electric Ariya, blending sleek design with advanced EV technology.</p>", "Nissan's Electric Crossover.\nExperience the future of driving with the all-electric Ariya, blending sleek design with advanced EV technology.\n", "Nissan Ariya" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c7d8e9f0-a1b2-c3d4-5678-9012e3f4a5b6"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Luxury Electric Benchmark.</h3><p>The sedan that redefined electric performance, offering incredible acceleration, range, and technology.</p>", "Luxury Electric Benchmark.\nThe sedan that redefined electric performance, offering incredible acceleration, range, and technology.\n", "Tesla Model S" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c9d0e1f2-a3b4-c5d6-7890-1234e5f6a7b8"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Family-Friendly Crossover.</h3><p>Nissan's popular compact SUV, offering advanced safety features and a comfortable, versatile interior.</p>", "Family-Friendly Crossover.\nNissan's popular compact SUV, offering advanced safety features and a comfortable, versatile interior.\n", "Nissan Rogue" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d0e1f2a3-b4c5-d6e7-8901-2345f6a7b8c9"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Intelligent Midsize Sedan.</h3><p>A stylish sedan featuring available All-Wheel Drive and driver-assist technologies.</p>", "Intelligent Midsize Sedan.\nA stylish sedan featuring available All-Wheel Drive and driver-assist technologies.\n", "Nissan Altima" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d2a1b0c9-f8e7-b6a5-d4c3-b2a1d0e9f8e7"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Compact dimensions, grand aspirations.</h3><p>Agile and adventurous, the GLA combines SUV versatility with compact efficiency, perfect for navigating city streets or exploring scenic routes.</p>", "Compact dimensions, grand aspirations.\nAgile and adventurous, the GLA combines SUV versatility with compact efficiency, perfect for navigating city streets or exploring scenic routes.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d2e3f4a5-b6c7-d8e9-0123-4567f8a9b0c1"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Electric Technology Flagship.</h3><p>A bold vision of the future SAV, featuring sustainable luxury, groundbreaking tech, and exhilarating electric power.</p>", "Electric Technology Flagship.\nA bold vision of the future SAV, featuring sustainable luxury, groundbreaking tech, and exhilarating electric power.\n", "BMW iX SAV" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-d0e1-f234-567890a1b2c3"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Compact Versatility.</h3><p>A stylish and efficient compact SUV offering flexibility for city driving and weekend getaways.</p>", "Compact Versatility.\nA stylish and efficient compact SUV offering flexibility for city driving and weekend getaways.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d6e7f8a9-b0c1-d2e3-4567-8901f2a3b4c5"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Legendary Performance.</h3><p>The iconic sports car returns, pairing timeless design with thrilling twin-turbo V6 power.</p>", "Legendary Performance.\nThe iconic sports car returns, pairing timeless design with thrilling twin-turbo V6 power.\n", "Nissan Z" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d8e9f0a1-b2c3-d4e5-6789-0123f4a5b6c7"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Electric SUV with Falcon Wings.</h3><p>A unique family SUV featuring distinctive Falcon Wing doors, impressive range, and performance.</p>", "Electric SUV with Falcon Wings.\nA unique family SUV featuring distinctive Falcon Wing doors, impressive range, and performance.\n", "Tesla Model X" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e1f2a3b4-c5d6-e7f8-9012-3456a7b8c9d0"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Sharp Compact Sedan.</h3><p>Offers unexpected style, standard safety features, and premium feel in the compact sedan class.</p>", "Sharp Compact Sedan.\nOffers unexpected style, standard safety features, and premium feel in the compact sedan class.\n", "Nissan Sentra" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e3b2a1d0-c9f8-e7b6-a5d4-c3b2a1d0e9f8"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>The future of luxury, electrified.</h3><p>The flagship electric sedan fuses progressive design with pioneering technology and breathtaking performance, setting a new standard for electric mobility.</p>", "The future of luxury, electrified.\nThe flagship electric sedan fuses progressive design with pioneering technology and breathtaking performance, setting a new standard for electric mobility.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e3f4a5b6-c7d8-e9f0-1234-5678a9b0c1d2"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>High-Performance Icon.</h3><p>The legendary M3 delivers uncompromising track-ready performance combined with everyday usability.</p>", "High-Performance Icon.\nThe legendary M3 delivers uncompromising track-ready performance combined with everyday usability.\n", "BMW M3 Sedan" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-e1f2-3456-7890a1b2c3d4"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Built Wild.</h3><p>Return of an icon. The Ford Bronco is engineered for rugged off-road capability and adventure.</p>", "Built Wild.\nReturn of an icon. The Ford Bronco is engineered for rugged off-road capability and adventure.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e7f8a9b0-c1d2-e3f4-5678-9012a3b4c5d6"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>The Ultimate Driving Machine.</h3><p>The quintessential sports sedan, balancing dynamic performance with everyday usability and luxury.</p>", "The Ultimate Driving Machine.\nThe quintessential sports sedan, balancing dynamic performance with everyday usability and luxury.\n", "BMW 3 Series Sedan" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e9f0a1b2-c3d4-e5f6-7890-1234a5b6c7d8"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Enhanced Electric Sport Sedan.</h3><p>Takes the Model 3 foundation and adds quicker acceleration, track mode, and sportier tuning.</p>", "Enhanced Electric Sport Sedan.\nTakes the Model 3 foundation and adds quicker acceleration, track mode, and sportier tuning.\n", "Tesla Model 3 Performance" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f0a1b2c3-d4e5-f6a7-8901-2345b6c7d8e9"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Beyond Ludicrous Speed.</h3><p>Offers staggering acceleration figures, making it one of the quickest production cars ever built.</p>", "Beyond Ludicrous Speed.\nOffers staggering acceleration figures, making it one of the quickest production cars ever built.\n", "Tesla Model S Plaid" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f2a3b4c5-d6e7-f8a9-0123-4567b8c9d0e1"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Rugged 3-Row SUV.</h3><p>Return to rugged. The Pathfinder offers seating for up to eight and capable performance for family adventures.</p>", "Rugged 3-Row SUV.\nReturn to rugged. The Pathfinder offers seating for up to eight and capable performance for family adventures.\n", "Nissan Pathfinder" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f4a3b2c1-d0e9-f8a7-b6c5-d4e3f2a1b0c9"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>The pinnacle of electric luxury SUVs.</h3><p>Experience flagship comfort and groundbreaking technology in an all-electric SUV form, redefining sustainable luxury with space for up to seven.</p>", "The pinnacle of electric luxury SUVs.\nExperience flagship comfort and groundbreaking technology in an all-electric SUV form, redefining sustainable luxury with space for up to seven.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f4a5b6c7-d8e9-f0a1-2345-6789b0c1d2e3"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Open-Top Freedom.</h3><p>A classic roadster experience with modern BMW performance, agility, and style.</p>", "Open-Top Freedom.\nA classic roadster experience with modern BMW performance, agility, and style.\n", "BMW Z4 Roadster" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-f2a3-4567-8901b2c3d4e5"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Electric Thrills.</h3><p>An all-electric SUV bearing the Mustang name, delivering exhilarating performance and advanced technology.</p>", "Electric Thrills.\nAn all-electric SUV bearing the Mustang name, delivering exhilarating performance and advanced technology.\n" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f8a9b0c1-d2e3-f4a5-6789-0123b4c5d6e7"),
                columns: new[] { "DescriptionHTML", "DescriptionText", "Name" },
                values: new object[] { "<h3>Versatile Sport Activity Vehicle.</h3><p>Combines dynamic BMW driving characteristics with the versatility and utility of an SAV.</p>", "Versatile Sport Activity Vehicle.\nCombines dynamic BMW driving characteristics with the versatility and utility of an SAV.\n", "BMW X3 SAV" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f9f8e7b6-a5d4-c3b2-a1d0-e9f8e7b6a5d4"),
                columns: new[] { "DescriptionHTML", "DescriptionText" },
                values: new object[] { "<h3>Open-air freedom, elegant design.</h3><p>Experience the joy of driving with the top down in the CLE Cabriolet, offering sophisticated style, year-round comfort, and exhilarating performance.</p>", "Open-air freedom, elegant design.\nExperience the joy of driving with the top down in the CLE Cabriolet, offering sophisticated style, year-round comfort, and exhilarating performance.\n" });
        }
    }
}
