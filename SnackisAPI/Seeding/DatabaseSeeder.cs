using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SnackisAPI.Data;
using SnackisAPI.Helpers;
using SnackisAPI.Models;
using SnackisAPI.Settings;

namespace SnackisAPI.Seeding;

public class DatabaseSeeder(
    SnackisDbContext context,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<SeedSettings> seedOptions,
    ILogger<DatabaseSeeder> logger)
{
    private const string AdminRoleName = "Admin";
    private const string UserRoleName = "User";

    private readonly SeedSettings seedSettings = seedOptions.Value;

    public async Task SeedAsync()
    {
        await EnsureRolesExistAsync();
        await EnsureAdminExistsAsync();
        await SeedExampleContentAsync();
    }

    private async Task EnsureRolesExistAsync()
    {
        foreach (var roleName in new[] { AdminRoleName, UserRoleName })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    private async Task EnsureAdminExistsAsync()
    {
        if (string.IsNullOrWhiteSpace(seedSettings.AdminPassword))
        {
            logger.LogWarning("Seed:AdminPassword is not set, no admin account is created.");
            return;
        }

        if (await userManager.FindByNameAsync(seedSettings.AdminUserName) is not null)
            return;

        var admin = new User { UserName = seedSettings.AdminUserName, Email = seedSettings.AdminEmail };
        await CreateUserAsync(admin, seedSettings.AdminPassword, AdminRoleName, UserRoleName);
        logger.LogInformation("Admin account {UserName} created", admin.UserName);
    }

    private async Task SeedExampleContentAsync()
    {
        if (await context.Topics.AnyAsync())
            return;

        if (string.IsNullOrWhiteSpace(seedSettings.DemoPassword))
        {
            logger.LogWarning("Seed:DemoPassword is not set, no example content is created.");
            return;
        }

        var anna = await CreateDemoUserAsync("anna");
        var erik = await CreateDemoUserAsync("erik");
        var maja = await CreateDemoUserAsync("maja");
        var olle = await CreateDemoUserAsync("olle");

        var vehicles = NewTopic("Fordonssnack");
        var cars = NewTopic("Bilsnack", vehicles);
        var boats = NewTopic("Båtsnack", vehicles);
        var bikes = NewTopic("Cykelsnack", vehicles);
        var food = NewTopic("Matsnack");
        var recipes = NewTopic("Recept", food);
        var restaurants = NewTopic("Restauranger", food);
        var tech = NewTopic("Teknik");
        var phones = NewTopic("Mobiler", tech);
        var computers = NewTopic("Datorer", tech);

        var familyCar = NewPost("Vilken bil är bäst för en barnfamilj?", "Vi är fyra i familjen och behöver en bil med plats för barnvagn och lite packning. Budget runt 250 000 kr. Vad skulle ni välja?", anna, cars, daysAgo: 6);
        var electricCar = NewPost("Elbil eller diesel på lång sikt?", "Jag kör ungefär 3 000 mil per år. Är elbil verkligen billigare om man räknar på fem år, eller är dieseln fortfarande ett bra val?", erik, cars, daysAgo: 4);
        var firstBoat = NewPost("Tips på första båt för skärgården", "Vi vill ha en liten båt för dagsturer i skärgården. Jag funderar på en öppen motorbåt på 18 fot. Vad bör man tänka på innan man köper?", olle, boats, daysAgo: 5);
        var coastRoute = NewPost("Bästa cykelrutten längs kusten", "Planerar en tvådagarstur med cykel längs kusten i sommar. Någon som har tips på etapper, fika och boende längs vägen?", maja, bikes, daysAgo: 3);
        var pasta = NewPost("Snabb pasta med citron och parmesan", "Min favoriträtt när jag har dåligt om tid: spagetti, olivolja, citron, mycket parmesan och svartpeppar. Färdig på tio minuter!", maja, recipes, daysAgo: 2);
        var italian = NewPost("Någon som testat nya italienaren på torget?", "Det öppnade en ny italiensk restaurang på torget förra veckan. Är pizzan lika bra som den ser ut? Och går det att boka bord på helgen?", erik, restaurants, daysAgo: 1);
        var longestPhone = NewPost("Vilken mobil håller längst?", "Min gamla telefon börjar bli långsam efter fyra år. Vilken mobil har ni haft längst och varit nöjda med? Gärna med bra uppdateringar.", olle, phones, daysAgo: 2);
        var buildPc = NewPost("Bygga dator själv eller köpa färdig?", "Jag funderar på en ny dator för spel och lite programmering. Är det värt att bygga själv, eller är en färdig dator smidigare?", anna, computers, daysAgo: 1);

        var comments = new[]
        {
            NewComment(familyCar, erik, "Jag skulle titta på en kombi, det blir mycket mer plats för vagnen än i en vanlig sedan.", hoursAfterPost: 3),
            NewComment(familyCar, maja, "Kolla gärna på begagnade tvååringar, då slipper du största värdeminskningen.", hoursAfterPost: 9),
            NewComment(electricCar, anna, "Vi bytte till elbil i fjol och laddar hemma på natten. Det har blivit klart billigare.", hoursAfterPost: 5),
            NewComment(electricCar, olle, "Det beror mycket på om du har möjlighet att ladda hemma. Annars blir det dyrare.", hoursAfterPost: 12),
            NewComment(firstBoat, maja, "Tänk på förvaring och trailer också, det glöms bort lätt!", hoursAfterPost: 4),
            NewComment(coastRoute, olle, "Etappen mellan fiskeläget och fyren är otroligt fin. Ta med fika.", hoursAfterPost: 6),
            NewComment(pasta, anna, "Gjorde den igår, supergott! Jag la i lite chiliflingor också.", hoursAfterPost: 2),
            NewComment(pasta, erik, "Enkelt och gott, tack för tipset.", hoursAfterPost: 7),
            NewComment(italian, maja, "Jag var där i lördags. Pizzan var fantastisk, men boka i förväg.", hoursAfterPost: 5),
            NewComment(longestPhone, erik, "Min senaste har hållit i sex år och fungerar fortfarande bra.", hoursAfterPost: 3),
            NewComment(buildPc, olle, "Bygg själv om du tycker det är kul, annars är färdig dator helt okej.", hoursAfterPost: 4),
            NewComment(buildPc, maja, "Det är faktiskt inte så svårt som det ser ut. Det finns bra guider.", hoursAfterPost: 8)
        };

        var messages = new[]
        {
            NewMessage(anna, erik, "Hej! Såg din fråga om elbilar. Vi kan prata mer om du vill, vi har kört elbil i ett år.", daysAgo: 3),
            NewMessage(erik, anna, "Tack, gärna! Hur mycket kostade det att installera laddboxen?", daysAgo: 3),
            NewMessage(maja, olle, "Tack för tipset om fyren, vi tog den vägen i helgen!", daysAgo: 1)
        };

        var report = new Report
        {
            Post = longestPhone,
            User = erik,
            Reason = "Misstänkt reklam för en viss mobiltillverkare.",
            Date = DateTime.UtcNow.AddHours(-20)
        };

        context.Topics.AddRange(vehicles, cars, boats, bikes, food, recipes, restaurants, tech, phones, computers);
        context.Posts.AddRange(familyCar, electricCar, firstBoat, coastRoute, pasta, italian, longestPhone, buildPc);
        context.Comments.AddRange(comments);
        context.PrivateMessages.AddRange(messages);
        context.Reports.Add(report);
        await context.SaveChangesAsync();

        logger.LogInformation("Example content created: 10 topics, 8 posts, {CommentCount} comments", comments.Length);
    }

    private async Task<User> CreateDemoUserAsync(string userName)
    {
        var user = new User { UserName = userName, Email = $"{userName}@snackis.local" };
        await CreateUserAsync(user, seedSettings.DemoPassword, UserRoleName);
        return user;
    }

    private async Task CreateUserAsync(User user, string password, params string[] roleNames)
    {
        var creationResult = await userManager.CreateAsync(user, password);
        if (!creationResult.Succeeded)
        {
            var errorDescriptions = string.Join(" ", creationResult.Errors.Select(error => error.Description));
            throw new InvalidOperationException($"Could not create seed user {user.UserName}: {errorDescriptions}");
        }

        await userManager.AddToRolesAsync(user, roleNames);
    }

    private static Topic NewTopic(string name, Topic? parent = null) => new()
    {
        Name = name,
        UrlName = UrlFormatter.ToCleanUrl(name),
        Parent = parent
    };

    private static Post NewPost(string title, string content, User author, Topic topic, int daysAgo) => new()
    {
        Title = title,
        Content = content,
        User = author,
        Topic = topic,
        Date = DateTime.UtcNow.AddDays(-daysAgo)
    };

    private static Comment NewComment(Post post, User author, string content, int hoursAfterPost) => new()
    {
        Post = post,
        User = author,
        Content = content,
        Date = post.Date.AddHours(hoursAfterPost)
    };

    private static PrivateMessage NewMessage(User sender, User receiver, string content, int daysAgo) => new()
    {
        Sender = sender,
        Receiver = receiver,
        Content = content,
        Date = DateTime.UtcNow.AddDays(-daysAgo)
    };
}
