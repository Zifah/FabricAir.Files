namespace FabricAir.Files.Api.Data;
public class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        SeedRoles(context);
        SeedFileGroups(context);
        SeedUsers(context);
        SeedFiles(context);
    }

    private static void SeedRoles(ApplicationDbContext context)
    {
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role("Administrator", "This user has access to everything"),
                new Role("Salesperson", "A user who deals with clients"),
                new Role("Web designer", "A user who creates the look and feel of the website")
            );
            context.SaveChanges();
        }
    }

    private static void SeedFileGroups(ApplicationDbContext context)
    {
        if (!context.FileGroups.Any())
        {
            var administrators = context.Roles.Single(r => r.Name == "Administrator");
            var salespersons = context.Roles.Single(r => r.Name == "Salesperson");
            var webDesigners = context.Roles.Single(r => r.Name == "Web designer");

            var pictureGroup = new FileGroup("Pictures");
            var manualsGroup = new FileGroup("Manuals");
            var drawingsGroup = new FileGroup("Drawings");
            var reportsGroup = new FileGroup("Reports");

            administrators.FileGroups.Add(pictureGroup);
            administrators.FileGroups.Add(manualsGroup);
            administrators.FileGroups.Add(drawingsGroup);
            administrators.FileGroups.Add(reportsGroup);

            salespersons.FileGroups.Add(manualsGroup);
            salespersons.FileGroups.Add(drawingsGroup);
            salespersons.FileGroups.Add(reportsGroup);

            webDesigners.FileGroups.Add(pictureGroup);
            webDesigners.FileGroups.Add(manualsGroup);

            context.SaveChanges();
        }
    }

    private static void SeedUsers(ApplicationDbContext context)
    {
        if (!context.Users.Any())
        {
            var roles = context.Roles.ToList();
            var random = new Random();

            for (int i = 1; i <= 10; i++)
            {
                context.Users.Add(new User($"User{i}", $"LastName{i}", $"user{i}@example.com", $"password{i}")
                {
                    Roles = roles.OrderBy(_ => Guid.NewGuid()).Take(random.Next(1, 3)).ToList()
                });
            }

            context.SaveChanges();
        }
    }

    private static void SeedFiles(ApplicationDbContext context)
    {
        if (!context.Files.Any())
        {
            var fileGroups = context.FileGroups.ToList();
            var random = new Random();

            var pictureExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };

            for (int i = 1; i <= 50; i++)
            {
                var randomFileGroup = fileGroups[random.Next(fileGroups.Count)];

                var url = string.Empty;
                if (randomFileGroup.Name == "Reports" || randomFileGroup.Name == "Drawings" || randomFileGroup.Name == "Manuals")
                {
                    url = $"https://example.com/file_{i}.pdf";
                }
                else if (randomFileGroup.Name == "Pictures")
                {
                    var randomPictureExtension = pictureExtensions[random.Next(pictureExtensions.Count)];
                    url = $"https://example.com/file_{i}{randomPictureExtension}";
                }

                context.Files.Add(new File($"File_{i}", randomFileGroup.Id, url));
            }

            context.SaveChanges();
        }
    }

}
