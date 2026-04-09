namespace AI.Boilerplate.Server.Api.Features.Products;

public partial class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(t => t.HasComment("产品表"));
        builder.Property(p => p.Id).HasComment("主键ID");
        builder.Property(p => p.ShortId).HasComment("短编号");
        builder.Property(p => p.Name).HasComment("产品名称");
        builder.Property(p => p.Price).HasComment("产品价格");
        builder.Property(p => p.DescriptionHTML).HasComment("HTML 描述");
        builder.Property(p => p.DescriptionText).HasComment("纯文本描述");
        builder.Property(p => p.CreatedOn).HasComment("创建时间");
        builder.Property(p => p.CategoryId).HasComment("类别 ID");
        builder.Property(p => p.Version).HasComment("并发版本");
        builder.Property(p => p.HasPrimaryImage).HasComment("是否有主图");
        builder.Property(p => p.PrimaryImageAltText).HasComment("主图替代文本");

        builder.HasIndex(p => p.Name).IsUnique(); // 产品名称索引
        builder.HasIndex(p => p.ShortId).IsUnique(); // 短编号索引

        builder.Property(p => p.ShortId).UseSequence("产品短编号");
        if (AppDbContext.IsEmbeddingEnabled)
        {
            builder.Property(p => p.Embedding).HasComment("语义向量");
            builder.Property(p => p.Embedding).HasColumnType("vector(768)"); // 维度取决于所使用的模型，这里假设为 768，因为使用的是 LocalTextEmbeddingGenerationService。
        }
        else
        {
            builder.Ignore(p => p.Embedding); // 忽略嵌入向量属性
        }


        var defaultVersion = 1;
        DateTimeOffset baseDate = DateTimeOffset.Parse("2026-03-21", styles: DateTimeStyles.AssumeUniversal); // 基准日期

        // --- 奔驰条目 (19 辆车) ---
        // https://www.mercedes-benz.ca/en/all-vehicles

        var benzId = Guid.Parse("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08");
        builder.HasData(new Product
        {
            Id = Guid.Parse($"9a59dda2-7b12-4cc1-9658-d2586eef91d7"),
            Name = "EQB SUV",
            Price = 54_200M,
            DescriptionHTML = "<h3>探索的范围。可容纳多达 7 人。</h3><p>这是一款多功能 SUV，可容纳多达七人。是一款您可以每天享受的先进电动车。智能技术和贴心的奢华以快速响应和静音精致呈现。</p>",
            DescriptionText = "探索的范围。可容纳多达 7 人。\n这是一款多功能 SUV，可容纳多达七人。是一款您可以每天享受的先进电动车。智能技术和贴心的奢华以快速响应和静音精致呈现。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10000,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"5746ae3d-5116-4774-9d55-0ff496e5186f"),
            Name = "EQE Sedan",
            Price = 76_050M,
            DescriptionHTML = "<h3>电动、必需、典范</h3><p>它是未来主义、前卫和新鲜的，但您知道它的核心价值观。不断完善的奢华。不断进步的创新。以及对您福祉的永恒奉献。也许没有哪款电动轿车能如此新颖而自然。</p>",
            DescriptionText = "电动、必需、典范\n它是未来主义、前卫和新鲜的，但您知道它的核心价值观。不断完善的奢华。不断进步的创新。以及对您福祉的永恒奉献。也许没有哪款电动轿车能如此新颖而自然。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10001,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"512eb70b-1d39-4845-88c0-fe19cd2d1979"),
            Name = "EQE SUV",
            Price = 79_050M,
            DescriptionHTML = "<h3>先进、适应性强、爱冒险。</h3><p>不仅仅是新，它可以在未来更新其先进性。不仅仅是贴心，它可以预测需求和愿望。超越未来，它可以更好地利用您的时间，并使您使用它的时间更美好。</p>",
            DescriptionText = "先进、适应性强、爱冒险。\n不仅仅是新，它可以在未来更新其先进性。不仅仅是贴心，它可以预测需求和愿望。超越未来，它可以更好地利用您的时间，并使您使用它的时间更美好。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10003,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"f4a3b2c1-d0e9-f8a7-b6c5-d4e3f2a1b0c9"),
            Name = "EQS SUV",
            Price = 136_000M,
            DescriptionHTML = "<h3>电动奢华 SUV 的巅峰。</h3><p>体验旗舰级舒适性和突破性技术，尽在全电动 SUV 形态中，重新定义可持续奢华，最多可容纳七人。</p>",
            DescriptionText = "电动奢华 SUV 的巅峰。\n体验旗舰级舒适性和突破性技术，尽在全电动 SUV 形态中，重新定义可持续奢华，最多可容纳七人。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10004,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"e3b2a1d0-c9f8-e7b6-a5d4-c3b2a1d0e9f8"),
            Name = "EQS Sedan",
            Price = 136_000M,
            DescriptionHTML = "<h3>奢华的未来，电动化。</h3><p>旗舰电动轿车将前卫设计与开创性技术和惊人性能相融合，为电动出行设定了新的标准。</p>",
            DescriptionText = "奢华的未来，电动化。\n旗舰电动轿车将前卫设计与开创性技术和惊人性能相融合，为电动出行设定了新的标准。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10005,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"d2a1b0c9-f8e7-b6a5-d4c3-b2a1d0e9f8e7"),
            Name = "GLA SUV",
            Price = 49_900M,
            DescriptionHTML = "<h3>紧凑的车身，宏大的抱负。</h3><p>灵活而富有冒险精神，GLA 将 SUV 的多功能性与紧凑型的高效完美结合，非常适合在城市街道或风景优美的路线中穿行。</p>",
            DescriptionText = "紧凑的车身，宏大的抱负。\n灵活而富有冒险精神，GLA 将 SUV 的多功能性与紧凑型的高效完美结合，非常适合在城市街道或风景优美的路线中穿行。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10006,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"c1b0a9f8-e7b6-a5d4-c3b2-a1d0e9f8e7b6"),
            Name = "GLB SUV",
            Price = 52_500M,
            DescriptionHTML = "<h3>多功能性与空间的完美结合。</h3><p>GLB 尺寸虽小，但提供了意想不到的宽敞空间，选配第三排座椅，使其成为家庭出游的灵活且友好的紧凑型 SUV。</p>",
            DescriptionText = "多功能性与空间的完美结合。\nGLB 尺寸虽小，但提供了意想不到的宽敞空间，选配第三排座椅，使其成为家庭出游的灵活且友好的紧凑型 SUV。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10007,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"b0a9f8e7-b6a5-d4c3-b2a1-d0e9f8e7b6a5"),
            Name = "GLC SUV",
            Price = 58_900M,
            DescriptionHTML = "<h3>智能驱动，令人印象深刻的设计。</h3><p>GLC SUV 在中型豪华车细分市场中为舒适性、技术和性能设定了基准，能够轻松适应您的驾驶需求。</p>",
            DescriptionText = "智能驱动，令人印象深刻的设计。\nGLC SUV 在中型豪华车细分市场中为舒适性、技术和性能设定了基准，能够轻松适应您的驾驶需求。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10008,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"a9f8e7b6-a5d4-c3b2-a1d0-e9f8e7b6a5d4"),
            Name = "GLC Coupe",
            Price = 63_500M,
            DescriptionHTML = "<h3>运动风格，SUV 实力。</h3><p>将轿跑的动感与 SUV 的多功能性相结合，GLC Coupe 在任何道路上都能展现强大的气场。</p>",
            DescriptionText = "运动风格，SUV 实力。\n将轿跑的动感与 SUV 的多功能性相结合，GLC Coupe 在任何道路上都能展现强大的气场。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10009,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"98e7b6a5-d4c3-b2a1-d0e9-f8e7b6a5d4c3"),
            Name = "GLE SUV",
            Price = 82_800M,
            DescriptionHTML = "<h3>每一种地形的主宰。</h3><p>智能、宽敞且强大的 GLE SUV 为家庭和冒险者提供尖端技术和奢华舒适。</p>",
            DescriptionText = "每一种地形的主宰。\n智能、宽敞且强大的 GLE SUV 为家庭和冒险者提供尖端技术和奢华舒适。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10010,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"87b6a5d4-c3b2-a1d0-e9f8-e7b6a5d4c3b2"),
            Name = "GLE Coupe",
            Price = 94_900M,
            DescriptionHTML = "<h3>运动气息，强劲性能。</h3><p>GLE Coupe 将 SUV 的肌肉感与轿跑的优雅线条相结合，带来令人振奋的性能和不容忽视的风格。</p>",
            DescriptionText = "运动气息，强劲性能。\nGLE Coupe 将 SUV 的肌肉感与轿跑的优雅线条相结合，带来令人振奋的性能和不容忽视的风格。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10011,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"76a5d4c3-b2a1-d0e9-f8e7-b6a5d4c3b2a1"),
            Name = "GLS SUV",
            Price = 115_100M,
            DescriptionHTML = "<h3>豪华 SUV 的 S 级。</h3><p>为多达七名乘客提供头等舱般的旅行体验，GLS 将强大的气场与无与伦比的奢华、空间和科技相结合。</p>",
            DescriptionText = "豪华 SUV 的 S 级。\n为多达七名乘客提供头等舱般的旅行体验，GLS 将强大的气场与无与伦比的奢华、空间和科技相结合。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10012,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"65d4c3b2-a1d0-e9f8-e7b6-a5d4c3b2a1d0"),
            Name = "G-CLASS SUV",
            Price = 180_000M,
            DescriptionHTML = "<h3>一个被重新工程的图标。</h3><p>一眼就能认出，永远充满能力。G-Class 仍然是定义奢华越野车的标杆，将永恒的设计与现代科技和强悍的性能相结合。</p>",
            DescriptionText = "一个被重新工程的图标。\n一眼就能认出，永远充满能力。G-Class 仍然是定义奢华越野车的标杆，将永恒的设计与现代科技和强悍的性能相结合。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10013,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"54c3b2a1-d0e9-f8e7-b6a5-d4c3b2a1d0e9"),
            Name = "E-Class Sedan",
            Price = 73_900M,
            DescriptionHTML = "<h3>品牌的核心，智能精炼。</h3><p>作为智能的杰作，E-Class Sedan 无缝融合了动感设计、奢华舒适和开创性的驾驶辅助系统。</p>",
            DescriptionText = "品牌的核心，智能精炼。\n作为智能的杰作，E-Class Sedan 无缝融合了动感设计、奢华舒适和开创性的驾驶辅助系统。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10014,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"43b2a1d0-e9f8-e7b6-a5d4-c3b2a1d0e9f8"),
            Name = "S-Class Sedan",
            Price = 140_000M,
            DescriptionHTML = "<h3>汽车欲望的巅峰。</h3><p>无懈可击的工程技术，S-Class Sedan 在安全、舒适和驾驶体验方面开创了创新，定义了奢华旅行。</p>",
            DescriptionText = "汽车欲望的巅峰。\n无懈可击的工程技术，S-Class Sedan 在安全、舒适和驾驶体验方面开创了创新，定义了奢华旅行。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10015,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"32a1d0e9-f8e7-b6a5-d4c3-b2a1d0e9f8e7"),
            Name = "CLA COUPE",
            Price = 50_500M,
            DescriptionHTML = "<h3>从每个角度都无法抗拒。</h3><p>凭借标志性的倾斜车顶线和运动姿态，CLA Coupe 以富有表现力的设计和灵活的性能吸引着人们的目光。</p>",
            DescriptionText = "从每个角度都无法抗拒。\n凭借标志性的倾斜车顶线和运动姿态，CLA Coupe 以富有表现力的设计和灵活的性能吸引着人们的目光。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10016,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"21d0e9f8-e7b6-a5d4-c3b2-a1d0e9f8e7b6"),
            Name = "CLE Coupe",
            Price = 65_000M,
            DescriptionHTML = "<h3>运动优雅，重新定义。</h3><p>全新 CLE Coupe 将富有表现力的设计与动态操控和先进技术相结合，创造出现代的欲望图标。</p>",
            DescriptionText = "运动优雅，重新定义。\n全新 CLE Coupe 将富有表现力的设计与动态操控和先进技术相结合，创造出现代的欲望图标。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10017,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"10e9f8e7-b6a5-d4c3-b2a1-d0e9f8e7b6a5"),
            Name = "Mercedes-AMG GT Coupe",
            Price = 150_000M,
            DescriptionHTML = "<h3>提升驾驶性能。</h3><p>生于赛道，AMG GT Coupe 体现了纯粹跑车的本质，拥有惊人的动力和精准的操控。</p>",
            DescriptionText = "提升驾驶性能。\n生于赛道，AMG GT Coupe 体现了纯粹跑车的本质，拥有惊人的动力和精准的操控。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10018,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse($"f9f8e7b6-a5d4-c3b2-a1d0-e9f8e7b6a5d4"),
            Name = "CLE Cabriolet",
            Price = 75_000M,
            DescriptionHTML = "<h3>开放式驾驶的自由，优雅的设计。</h3><p>体验敞篷驾驶的乐趣，CLE Cabriolet 提供精致的风格、四季舒适和令人振奋的性能。</p>",
            DescriptionText = "开放式驾驶的自由，优雅的设计。\n体验敞篷驾驶的乐趣，CLE Cabriolet 提供精致的风格、四季舒适和令人振奋的性能。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = benzId,
            ShortId = 10019,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        // --- Ford 条目 (8 辆车) ---

        var fordId = Guid.Parse("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845");
        builder.HasData(new Product
        {
            Id = Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef"),
            Name = "Ford F-150",
            Price = 45_000M,
            DescriptionHTML = "<h3>福特铸造的坚韧。</h3><p>美国最畅销的卡车，以其工作或娱乐的能力、创新和坚韧而闻名。</p>",
            DescriptionText = "福特铸造的坚韧。\n美国最畅销的卡车，以其工作或娱乐的能力、创新和坚韧而闻名。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = fordId,
            ShortId = 10020,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("b2c3d4e5-f6a7-89b0-12c3-45d678e9f0a1"),
            Name = "Ford Mustang",
            Price = 40_000M,
            DescriptionHTML = "<h3>标志性的美式肌肉车。</h3><p>激动人心的性能和无可否认的风格定义了传奇的福特野马轿跑。</p>",
            DescriptionText = "标志性的美式肌肉车。\n激动人心的性能和无可否认的风格定义了传奇的福特野马轿跑。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = fordId,
            ShortId = 10021,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("c3d4e5f6-a7b8-9c01-23d4-56e789f0a1b2"),
            Name = "Ford Explorer",
            Price = 48_000M,
            DescriptionHTML = "<h3>冒险准备好的 SUV。</h3><p>一款宽敞且功能强大的 SUV，专为家庭冒险而设计，提供三排座椅和现代科技。</p>",
            DescriptionText = "冒险准备好的 SUV。\n一款宽敞且功能强大的 SUV，专为家庭冒险而设计，提供三排座椅和现代科技。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = fordId,
            ShortId = 10022,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("d4e5f6a7-b8c9-d0e1-f234-567890a1b2c3"),
            Name = "Ford Escape",
            Price = 35_000M,
            DescriptionHTML = "<h3>紧凑型多功能性。</h3><p>一款时尚且高效的紧凑型 SUV，提供城市驾驶和周末度假的灵活性。</p>",
            DescriptionText = "紧凑型多功能性。\n一款时尚且高效的紧凑型 SUV，提供城市驾驶和周末度假的灵活性。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = fordId,
            ShortId = 10023,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("e5f6a7b8-c9d0-e1f2-3456-7890a1b2c3d4"),
            Name = "Ford Bronco",
            Price = 42_000M,
            DescriptionHTML = "<h3>野性重生。</h3><p>图标的回归。福特 Bronco 专为强悍的越野能力和冒险而生。</p>",
            DescriptionText = "野性重生。\n图标的回归。福特 Bronco 专为强悍的越野能力和冒险而生。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = fordId,
            ShortId = 10024,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("f6a7b8c9-d0e1-f2a3-4567-8901b2c3d4e5"),
            Name = "Ford Mustang Mach-E",
            Price = 55_000M,
            DescriptionHTML = "<h3>电动惊喜。</h3><p>一款全电动 SUV，承载着野马的名字，带来令人振奋的性能和先进的科技。</p>",
            DescriptionText = "电动惊喜。\n一款全电动 SUV，承载着野马的名字，带来令人振奋的性能和先进的科技。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = fordId,
            ShortId = 10025,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("a7b8c9d0-e1f2-a3b4-5678-9012c3d4e5f6"),
            Name = "Ford Maverick",
            Price = 28_000M,
            DescriptionHTML = "<h3>紧凑型卡车，大构想。</h3><p>一款经济实惠且多功能的紧凑型皮卡，标配混合动力系统。</p>",
            DescriptionText = "紧凑型卡车，大构想。\n一款经济实惠且多功能的紧凑型皮卡，标配混合动力系统。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = fordId,
            ShortId = 10026,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("b8c9d0e1-f2a3-b4c5-6789-0123d4e5f6a7"),
            Name = "Ford Edge",
            Price = 40_000M,
            DescriptionHTML = "<h3>时尚的中型跨界车。</h3><p>将精致的设计、智能科技和动感性能结合在一起，形成一款两厢跨界车。</p>",
            DescriptionText = "时尚的中型跨界车。\n将精致的设计、智能科技和动感性能结合在一起，形成一款两厢跨界车。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = fordId,
            ShortId = 10027,
            Version = defaultVersion,
            HasPrimaryImage = false
        });


        // --- 日产条目 (8 辆车) ---

        var nissanId = Guid.Parse("582b8c19-0709-4dae-b7a6-fa0e704dad3c");
        builder.HasData(new Product
        {
            Id = Guid.Parse("c9d0e1f2-a3b4-c5d6-7890-1234e5f6a7b8"),
            Name = "日产 Rogue",
            Price = 32_000M,
            DescriptionHTML = "<h3>适合家庭的跨界车。</h3><p>日产畅销的紧凑型 SUV，提供先进的安全功能和舒适多变的内饰。</p>",
            DescriptionText = "适合家庭的跨界车。\n日产畅销的紧凑型 SUV，提供先进的安全功能和舒适多变的内饰。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = nissanId,
            ShortId = 10028,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("d0e1f2a3-b4c5-d6e7-8901-2345f6a7b8c9"),
            Name = "日产 Altima",
            Price = 30_000M,
            DescriptionHTML = "<h3>智能中型轿车。</h3><p>一款时尚的轿车，配备可选的全轮驱动和驾驶辅助技术。</p>",
            DescriptionText = "智能中型轿车。\n一款时尚的轿车，配备可选的全轮驱动和驾驶辅助技术。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = nissanId,
            ShortId = 10029,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("e1f2a3b4-c5d6-e7f8-9012-3456a7b8c9d0"),
            Name = "日产 Sentra",
            Price = 25_000M,
            DescriptionHTML = "<h3>锋利的紧凑型轿车。</h3><p>在紧凑轿车级别中提供意想不到的风格、标准安全功能和优质感受。</p>",
            DescriptionText = "锋利的紧凑型轿车。\n在紧凑轿车级别中提供意想不到的风格、标准安全功能和优质感受。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = nissanId,
            ShortId = 10030,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("f2a3b4c5-d6e7-f8a9-0123-4567b8c9d0e1"),
            Name = "日产 Pathfinder",
            Price = 40_000M,
            DescriptionHTML = "<h3>坚固的 3 排 SUV。</h3><p>重返坚固。Pathfinder 提供多达八个座位和强大的性能，适合家庭冒险。</p>",
            DescriptionText = "坚固的 3 排 SUV。\n重返坚固。Pathfinder 提供多达八个座位和强大的性能，适合家庭冒险。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = nissanId,
            ShortId = 10031,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("a3b4c5d6-e7f8-a9b0-1234-5678c9d0e1f2"),
            Name = "日产 Frontier",
            Price = 35_000M,
            DescriptionHTML = "<h3>合适尺寸的皮卡。</h3><p>一款坚固耐用的中型卡车，旨在满足工作需求和周末冒险。</p>",
            DescriptionText = "合适尺寸的皮卡。\n一款坚固耐用的中型卡车，旨在满足工作需求和周末冒险。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = nissanId,
            ShortId = 10032,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("b4c5d6e7-f8a9-b0c1-2345-6789d0e1f2a3"),
            Name = "日产 Kicks",
            Price = 24_000M,
            DescriptionHTML = "<h3>富有表现力的小型跨界车。</h3><p>通过可定制的风格脱颖而出，享受适合城市的灵活性和智能科技。</p>",
            DescriptionText = "富有表现力的小型跨界车。\n通过可定制的风格脱颖而出，享受适合城市的灵活性和智能科技。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = nissanId,
            ShortId = 10033,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("c5d6e7f8-a9b0-c1d2-3456-7890e1f2a3b4"),
            Name = "日产 Ariya",
            Price = 50_000M,
            DescriptionHTML = "<h3>日产的电动跨界车。</h3><p>体验未来驾驶，尽在全电动 Ariya 中，融合流线型设计和先进的电动汽车技术。</p>",
            DescriptionText = "日产的电动跨界车。\n体验未来驾驶，尽在全电动 Ariya 中，融合流线型设计和先进的电动汽车技术。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = nissanId,
            ShortId = 10034,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("d6e7f8a9-b0c1-d2e3-4567-8901f2a3b4c5"),
            Name = "日产 Z",
            Price = 45_000M,
            DescriptionHTML = "<h3>传奇性能。</h3><p>标志性的跑车回归，结合了永恒的设计和令人兴奋的双涡轮增压 V6 动力。</p>",
            DescriptionText = "传奇性能。\n标志性的跑车回归，结合了永恒的设计和令人兴奋的双涡轮增压 V6 动力。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = nissanId,
            ShortId = 10035,
            Version = defaultVersion,
            HasPrimaryImage = false
        });


        // --- 宝马条目 (8 辆车) ---
        var bmwId = Guid.Parse("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4");
        builder.HasData(new Product
        {
            Id = Guid.Parse("e7f8a9b0-c1d2-e3f4-5678-9012a3b4c5d6"),
            Name = "宝马 3 系列轿车",
            Price = 50_000M,
            DescriptionHTML = "<h3>终极驾驶机器。</h3><p>典型的运动轿车，平衡了动态性能、日常实用性和奢华感。</p>",
            DescriptionText = "终极驾驶机器。\n典型的运动轿车，平衡了动态性能、日常实用性和奢华感。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = bmwId,
            ShortId = 10036,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("f8a9b0c1-d2e3-f4a5-6789-0123b4c5d6e7"),
            Name = "宝马 X3 SAV",
            Price = 55_000M,
            DescriptionHTML = "<h3>多才多艺的运动型活动车。</h3><p>将动态的宝马驾驶特性与 SAV 的多功能性和实用性相结合。</p>",
            DescriptionText = "多才多艺的运动型活动车。\n将动态的宝马驾驶特性与 SAV 的多功能性和实用性相结合。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = bmwId,
            ShortId = 10037,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("a9b0c1d2-e3f4-a5b6-7890-1234c5d6e7f8"),
            Name = "宝马 X5 SAV",
            Price = 70_000M,
            DescriptionHTML = "<h3>老板。</h3><p>原始的运动型活动车，在其细分市场中设定了奢华、性能和能力的基准。</p>",
            DescriptionText = "老板。\n原始的运动型活动车，在其细分市场中设定了奢华、性能和能力的基准。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = bmwId,
            ShortId = 10038,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("b0c1d2e3-f4a5-b6c7-8901-2345d6e7f8a9"),
            Name = "宝马 5 系列轿车",
            Price = 65_000M,
            DescriptionHTML = "<h3>行政级运动感。</h3><p>将动态性能、尖端科技和奢华舒适完美融合，为行政级人士提供精致体验。</p>",
            DescriptionText = "行政级运动感。\n将动态性能、尖端科技和奢华舒适完美融合，为行政级人士提供精致体验。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = bmwId,
            ShortId = 10039,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("c1d2e3f4-a5b6-c7d8-9012-3456e7f8a9b0"),
            Name = "宝马 i4 Gran Coupe",
            Price = 60_000M,
            DescriptionHTML = "<h3>电动性能轿跑。</h3><p>宝马首款全电动 Gran Coupe，提供令人印象深刻的续航里程和标志性的宝马驾驶动态。</p>",
            DescriptionText = "电动性能轿跑。\n宝马首款全电动 Gran Coupe，提供令人印象深刻的续航里程和标志性的宝马驾驶动态。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = bmwId,
            ShortId = 10040,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("d2e3f4a5-b6c7-d8e9-0123-4567f8a9b0c1"),
            Name = "宝马 iX SAV",
            Price = 90_000M,
            DescriptionHTML = "<h3>电动科技旗舰。</h3><p>未来 SAV 的大胆愿景，具有可持续奢华、突破性科技和令人振奋的电动动力。</p>",
            DescriptionText = "电动科技旗舰。\n未来 SAV 的大胆愿景，具有可持续奢华、突破性科技和令人振奋的电动动力。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = bmwId,
            ShortId = 10041,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("e3f4a5b6-c7d8-e9f0-1234-5678a9b0c1d2"),
            Name = "宝马 M3 Sedan",
            Price = 80_000M,
            DescriptionHTML = "<h3>高性能图标。</h3><p>传奇的 M3 提供无与伦比的赛道准备性能，同时兼顾日常实用性。</p>",
            DescriptionText = "高性能图标。\n传奇的 M3 提供无与伦比的赛道准备性能，同时兼顾日常实用性。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = bmwId,
            ShortId = 10042,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("f4a5b6c7-d8e9-f0a1-2345-6789b0c1d2e3"),
            Name = "宝马 Z4 跑车",
            Price = 60_000M,
            DescriptionHTML = "<h3>敞篷自由。</h3><p>经典跑车体验，结合现代宝马性能、灵活性和风格。</p>",
            DescriptionText = "敞篷自由。\n经典跑车体验，结合现代宝马性能、灵活性和风格。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = bmwId,
            ShortId = 10043,
            Version = defaultVersion,
            HasPrimaryImage = false
        });


        // --- 特斯拉条目 (7 辆车) ---
        var teslaId = Guid.Parse("747f6d66-7524-40ca-8494-f65e85b5ee5d");
        builder.HasData(new Product
        {
            Id = Guid.Parse("a5b6c7d8-e9f0-a1b2-3456-7890c1d2e3f4"),
            Name = "特斯拉 Model 3",
            Price = 45_000M,
            DescriptionHTML = "<h3>大众市场电动轿车。</h3><p>特斯拉最实惠的车型，提供令人印象深刻的续航里程、性能和极简设计。</p>",
            DescriptionText = "大众市场电动轿车。\n特斯拉最实惠的车型，提供令人印象深刻的续航里程、性能和极简设计。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = teslaId,
            ShortId = 10044,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("b6c7d8e9-f0a1-b2c3-4567-8901d2e3f4a5"),
            Name = "特斯拉 Model Y",
            Price = 55_000M,
            DescriptionHTML = "<h3>紧凑型电动 SUV。</h3><p>一款多功能电动 SUV，提供充足的空间、性能和访问特斯拉超级充电网络的能力。</p>",
            DescriptionText = "紧凑型电动 SUV。\n一款多功能电动 SUV，提供充足的空间、性能和访问特斯拉超级充电网络的能力。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = teslaId,
            ShortId = 10045,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("c7d8e9f0-a1b2-c3d4-5678-9012e3f4a5b6"),
            Name = "特斯拉 Model S",
            Price = 90_000M,
            DescriptionHTML = "<h3>奢华电动基准。</h3><p>重新定义电动性能的轿车，提供令人难以置信的加速、续航和科技。</p>",
            DescriptionText = "奢华电动基准。\n重新定义电动性能的轿车，提供令人难以置信的加速、续航和科技。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = teslaId,
            ShortId = 10046,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("d8e9f0a1-b2c3-d4e5-6789-0123f4a5b6c7"),
            Name = "特斯拉 Model X",
            Price = 100_000M,
            DescriptionHTML = "<h3>带有鹰翼门的电动 SUV。</h3><p>一款独特的家庭 SUV，具有独特的鹰翼门、令人印象深刻的续航和性能。</p>",
            DescriptionText = "带有鹰翼门的电动 SUV。\n一款独特的家庭 SUV，具有独特的鹰翼门、令人印象深刻的续航和性能。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = teslaId,
            ShortId = 10047,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("e9f0a1b2-c3d4-e5f6-7890-1234a5b6c7d8"),
            Name = "特斯拉 Model 3 Performance",
            Price = 55_000M,
            DescriptionHTML = "<h3>增强版电动运动轿车。</h3><p>在 Model 3 的基础上，增加了更快的加速、赛道模式和更运动的调校。</p>",
            DescriptionText = "增强版电动运动轿车。\n在 Model 3 的基础上，增加了更快的加速、赛道模式和更运动的调校。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = teslaId,
            ShortId = 10048,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("f0a1b2c3-d4e5-f6a7-8901-2345b6c7d8e9"),
            Name = "特斯拉 Model S Plaid",
            Price = 110_000M,
            DescriptionHTML = "<h3>超越疯狂的速度。</h3><p>提供惊人的加速数据，使其成为有史以来最快的量产车之一。</p>",
            DescriptionText = "超越疯狂的速度。\n提供惊人的加速数据，使其成为有史以来最快的量产车之一。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = teslaId,
            ShortId = 10049,
            Version = defaultVersion,
            HasPrimaryImage = false
        });

        builder.HasData(new Product
        {
            Id = Guid.Parse("a1b2c3d4-e5f6-a7b8-9012-3456c7d8e9f0"),
            Name = "特斯拉 Cybertruck",
            Price = 70_000M,
            DescriptionHTML = "<h3>激进的电动皮卡。</h3><p>一款具有外骨骼设计的未来派皮卡，提供多功能性、性能和耐用性。</p>",
            DescriptionText = "激进的电动皮卡。\n一款具有外骨骼设计的未来派皮卡，提供多功能性、性能和耐用性。\n",
            CreatedOn = baseDate.AddDays(-10),
            CategoryId = teslaId,
            ShortId = 10050,
            Version = defaultVersion,
            HasPrimaryImage = false
        });
    }
}

