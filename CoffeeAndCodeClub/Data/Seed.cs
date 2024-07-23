using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WeCodeCoffee.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeCodeCoffee.Data;
using WeCodeCoffee.Data.Enum;

namespace WeCodeCoffee.Data
{
    public class Seed
    {
        public static async Task SeedDataAndUsersAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                // Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "johndoe@gmail.com";

                // Ensure the Admin role exists
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                // Ensure the User role exists
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                // Ensure the Admin user exists and is assigned the Admin role
                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "johndoe",
                        Bio = "Full stack developer with a knack for problem-solving.",
                        DeveloperType = "Full Stack Developer",
                        ProfilePictureUrl = "/images/profile1.jpg",
                        FirstName = "John",
                        LastName = "Doe",
                        LastLoginTime = DateTime.Now.AddDays(-3),
                        YearJoined = 2020,
                        ContributionScore = 180,
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "123 Main St",
                            City = "Charlotte",
                            State = "NC",
                            ZipCode = 49001,
                            Country = "USA"
                        }
                    };
                    var result = await userManager.CreateAsync(newAdminUser, "Password123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                    }
                }
                else
                {
                    // Ensure the user's email is correct and user is in the Admin role
                    if (adminUser.Email != adminUserEmail)
                    {
                        adminUser.Email = adminUserEmail;
                        adminUser.EmailConfirmed = true;
                        await userManager.UpdateAsync(adminUser);
                    }
                    if (!await userManager.IsInRoleAsync(adminUser, UserRoles.Admin))
                    {
                        await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                    }
                    else
                    {
                        // Remove and reassign the Admin role to ensure it
                        await userManager.RemoveFromRoleAsync(adminUser, UserRoles.Admin);
                        await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                    }
                }

                // Seed regular AppUsers
                if (!context.Users.Any(u => u.Email != adminUserEmail))
                {
                    var users = new List<AppUser>
                    {
                        new AppUser
                        {
                            UserName = "jameskyle",
                            FirstName = "James",
                            LastName = "Kyle",
                            Bio = "Passionate frontend developer with 5 years of experience.",
                            DeveloperType = "Frontend Developer",
                            ProfilePictureUrl = "/images/profile2.jpg",
                            LastLoginTime = DateTime.Now.AddDays(-1),
                            YearJoined = 2019,
                            ContributionScore = 150,
                            Email = "jameskyle@gmail.com",
                            EmailConfirmed = true,
                            Address = new Address
                            {
                                Street = "123 Frontend St",
                                City = "San Francisco",
                                State = "CA",
                                ZipCode = 94101,
                                Country = "USA"
                            }
                        },
                        new AppUser
                        {
                            UserName = "janedoe",
                            FirstName = "Jane",
                            LastName = "Doe",
                            Bio = "Experienced backend developer specializing in .NET technologies.",
                            DeveloperType = "Backend Developer",
                            ProfilePictureUrl = "/images/profile3.jpg",
                            LastLoginTime = DateTime.Now.AddDays(-2),
                            YearJoined = 2018,
                            ContributionScore = 200,
                            Email = "janedoe@gmail.com",
                            EmailConfirmed = true,
                            Address = new Address
                            {
                                Street = "456 Backend St",
                                City = "Seattle",
                                State = "WA",
                                ZipCode = 98101,
                                Country = "USA"
                            }
                        },
                        new AppUser
                        {
                            UserName = "samsmith",
                            FirstName = "Sam",
                            LastName = "Smith",
                            Bio = "Full stack developer with a knack for problem-solving.",
                            DeveloperType = "Full Stack Developer",
                            ProfilePictureUrl = "/images/profile4.jpg",
                            LastLoginTime = DateTime.Now.AddDays(-3),
                            YearJoined = 2020,
                            ContributionScore = 180,
                            Email = "samsmith@gmail.com",
                            EmailConfirmed = true,
                            Address = new Address
                            {
                                Street = "789 Fullstack St",
                                City = "Austin",
                                State = "TX",
                                ZipCode = 73301,
                                Country = "USA"
                            }
                        },
                        new AppUser
                        {
                            UserName = "emilyjones",
                            FirstName = "Emily",
                            LastName = "Jones",
                            Bio = "Data scientist with expertise in machine learning.",
                            DeveloperType = "Data Scientist",
                            ProfilePictureUrl = "/images/profile5.jpg",
                            LastLoginTime = DateTime.Now.AddDays(-4),
                            Email = "emilyjones@gmail.com",
                            EmailConfirmed = true,
                            Address = new Address
                            {
                                Street = "123 Data St",
                                City = "Austin",
                                State = "TX",
                                ZipCode = 73301,
                                Country = "USA"
                            }
                        },
                        new AppUser
                        {
                            UserName = "michaelbrown",
                            FirstName = "Michael",
                            LastName = "Brown",
                            Bio = "DevOps engineer focused on CI/CD pipelines.",
                            DeveloperType = "DevOps Engineer",
                            ProfilePictureUrl = "/images/profile6.jpg",
                            LastLoginTime = DateTime.Now.AddDays(-5),
                            Email = "michaelbrown@gmail.com",
                            EmailConfirmed = true,
                            Address = new Address
                            {
                                Street = "456 DevOps St",
                                City = "Austin",
                                State = "TX",
                                ZipCode = 73301,
                                Country = "USA"
                            }
                        },
                   
                    };

                    foreach (var user in users)
                    {
                        var existingUser = await userManager.FindByEmailAsync(user.Email);
                        if (existingUser == null)
                        {
                            var result = await userManager.CreateAsync(user, "Password123!");
                            if (result.Succeeded)
                            {
                                await userManager.AddToRoleAsync(user, UserRoles.User);
                            }
                        }
                    }

                    context.SaveChanges();
                }

                // Seed Clubs
                if (!context.Clubs.Any())
                {
                    context.Clubs.AddRange(new List<Club>()
                    {
                        new Club()
                        {
                            Title = "Frontend",
                            Image = "/images/stock.jpg",
                            Description = "Want to perfect your frontend skills? Join our advanced frontend developers club! Each week, we dive into complex design and development topics, work on challenging projects, and enhance our expertise together.",
                            ClubCategory = ClubCategory.FrontEnd,
                            Address = new Address()
                            {
                                Street = "123 Peach St",
                                City = "Tallahassee",
                                State = "FL",
                                ZipCode = 32301,
                                Country = "USA"
                            }
                        },
                        new Club()
                        {
                            Title = "Frontend",
                            Image = "/images/stock8.jpg",
                            Description = "Want to perfect your frontend skills? Join our advanced frontend developers club! Each week, we dive into complex design and development topics, work on challenging projects, and enhance our expertise together.",
                            ClubCategory = ClubCategory.FrontEnd,
                            Address = new Address()
                            {
                                Street = "321 Birch St",
                                City = "Frankfort",
                                State = "KY",
                                ZipCode = 40601,
                                Country = "USA"
                            }
                        },
                        new Club()
                        {
                            Title = "Backend",
                            Image = "/images/stock3.jpg",
                            Description = "Dive deep into backend development with our specialized club. We explore server-side programming, databases, and architecture to build robust backend systems.",
                            ClubCategory = ClubCategory.BackEnd,
                            Address = new Address()
                            {
                                Street = "654 Willow St",
                                City = "Baton Rouge",
                                State = "LA",
                                ZipCode = 70801,
                                Country = "USA"
                            }
                        },
                        new Club()
                        {
                            Title = "FullStack",
                            Image = "/images/stock9.jpg",
                            Description = "Master both frontend and backend technologies in our FullStack developers club. Join us to work on end-to-end projects and become a versatile developer.",
                            ClubCategory = ClubCategory.FullStack,
                            Address = new Address()
                            {
                                Street = "321 Elm St",
                                City = "Saint Paul",
                                State = "MN",
                                ZipCode = 55101,
                                Country = "USA"
                            }
                        },
                        new Club()
                        {
                            Title = "LeetCode",
                            Image = "/images/stock7.jpg",
                            Description = "Join our LeetCode club to tackle challenging coding problems and prepare for technical interviews. Sharpen your problem-solving skills with regular practice.",
                            ClubCategory = ClubCategory.LeetCode,
                            Address = new Address()
                            {
                                Street = "654 Maple St",
                                City = "Jackson",
                                State = "MS",
                                ZipCode = 39201,
                                Country = "USA"
                            }
                        },
                        new Club()
                        {
                            Title = "Frontend",
                            Image = "/images/stock13.jpg",
                            Description = "Want to perfect your frontend skills? Join our advanced frontend developers club! Each week, we dive into complex design and development topics, work on challenging projects, and enhance our expertise together.",
                            ClubCategory = ClubCategory.FrontEnd,
                            Address = new Address()
                            {
                                Street = "789 Walnut St",
                                City = "Jefferson City",
                                State = "MO",
                                ZipCode = 65101,
                                Country = "USA"
                            }
                        },
                        new Club()
                        {
                            Title = "Backend",
                            Image = "/images/stock3.jpg",
                            Description = "Dive deep into backend development with our specialized club. We explore server-side programming, databases, and architecture to build robust backend systems.",
                            ClubCategory = ClubCategory.BackEnd,
                            Address = new Address()
                            {
                                Street = "321 Cedar St",
                                City = "Helena",
                                State = "MT",
                                ZipCode = 59601,
                                Country = "USA"
                            }
                        },
                        new Club()
                        {
                            Title = "FullStack",
                            Image = "/images/stock9.jpg",
                            Description = "Master both frontend and backend technologies in our FullStack developers club. Join us to work on end-to-end projects and become a versatile developer.",
                            ClubCategory = ClubCategory.FullStack,
                            Address = new Address()
                            {
                                Street = "654 Oak St",
                                City = "Lincoln",
                                State = "NE",
                                ZipCode = 68501,
                                Country = "USA"
                            }
                        },
                        new Club()
                        {
                            Title = "LeetCode",
                            Image = "/images/stock8.jpg",
                            Description = "Join our LeetCode club to tackle challenging coding problems and prepare for technical interviews. Sharpen your problem-solving skills with regular practice.",
                            ClubCategory = ClubCategory.LeetCode,
                            Address = new Address()
                            {
                                Street = "789 Pine St",
                                City = "Carson City",
                                State = "NV",
                                ZipCode = 89701,
                                Country = "USA"
                            }
                        },
                        new Club()
                        {
                            Title = "Backend",
                            Image = "/images/stock5.jpg",
                            Description = "Dive deep into backend development with our specialized club. We explore server-side programming, databases, and architecture to build robust backend systems.",
                            ClubCategory = ClubCategory.BackEnd,
                            Address = new Address()
                            {
                                Street = "789 Elm St",
                                City = "Richmond",
                                State = "VA",
                                ZipCode = 23218,
                                Country = "USA"
                            }
                        },
                        new Club()
                        {
                            Title = "FullStack",
                            Image = "/images/stock11.jpg",
                            Description = "Master both frontend and backend technologies in our FullStack developers club. Join us to work on end-to-end projects and become a versatile developer.",
                            ClubCategory = ClubCategory.FullStack,
                            Address = new Address()
                            {
                                Street = "321 Cedar St",
                                City = "Olympia",
                                State = "WA",
                                ZipCode = 98501,
                                Country = "USA"
                            }
                        },
                        new Club()
                        {
                            Title = "LeetCode",
                            Image = "/images/stock12.jpg",
                            Description = "Join our LeetCode club to tackle challenging coding problems and prepare for technical interviews. Sharpen your problem-solving skills with regular practice.",
                            ClubCategory = ClubCategory.LeetCode,
                            Address = new Address()
                            {
                                Street = "654 Maple St",
                                City = "Charleston",
                                State = "WV",
                                ZipCode = 25301,
                                Country = "USA"
                            }
                        },
                        new Club()
                        {
                            Title = "Frontend",
                            Image = "/images/stock10.jpg",
                            Description = "Want to perfect your frontend skills? Join our advanced frontend developers club! Each week, we dive into complex design and development topics, work on challenging projects, and enhance our expertise together.",
                            ClubCategory = ClubCategory.FrontEnd,
                            Address = new Address()
                            {
                                Street = "789 Elm St",
                                City = "Madison",
                                State = "WI",
                                ZipCode = 53703,
                                Country = "USA"
                            }
                        },
                        new Club()
                        {
                            Title = "Backend",
                            Image = "/images/stock6.jpg",
                            Description = "Dive deep into backend development with our specialized club. We explore server-side programming, databases, and architecture to build robust backend systems.",
                            ClubCategory = ClubCategory.BackEnd,
                            Address = new Address()
                            {
                                Street = "321 Cedar St",
                                City = "Cheyenne",
                                State = "WY",
                                ZipCode = 82001,
                                Country = "USA"
                            }
                        }
                    });
                    context.SaveChanges();
                }

                // Seed Events
                if (!context.Events.Any())
                {
                    context.Events.AddRange(new List<Event>()
                    {
                        new Event()
                        {
                            Title = "Meetup",
                            Image = "/images/stock4.jpg",
                            Description = "Join our upcoming meetup! Connect with fellow developers over coffee, share experiences, and discuss the latest industry trends. Enhance your network and gain new insights. Don’t miss this opportunity to learn and connect!",
                            EventCategory = EventCategory.Meetup,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC",
                                ZipCode = 28202,
                                Country = "USA"
                            }
                        },
                        new Event()
                        {
                            Title = "Meetup",
                            Image = "/images/stock5.jpg",
                            Description = "Join our upcoming meetup! Connect with fellow developers over coffee, share experiences, and discuss the latest industry trends. Enhance your network and gain new insights. Don’t miss this opportunity to learn and connect!",
                            EventCategory = EventCategory.Meetup,
                            Address = new Address()
                            {
                                Street = "456 Maple St",
                                City = "Bismarck",
                                State = "ND",
                                ZipCode = 58501,
                                Country = "USA"
                            }
                        },
                        new Event()
                        {
                            Title = "Meetup",
                            Image = "/images/stock3.jpg",
                            Description = "Join our upcoming meetup! Connect with fellow developers over coffee, share experiences, and discuss the latest industry trends. Enhance your network and gain new insights. Don’t miss this opportunity to learn and connect!",
                            EventCategory = EventCategory.Meetup,
                            Address = new Address()
                            {
                                Street = "789 Elm St",
                                City = "Columbus",
                                State = "OH",
                                ZipCode = 43215,
                                Country = "USA"
                            }
                        },
                        new Event()
                        {
                            Title = "Meetup",
                            Image = "/images/stock2.jpg",
                            Description = "Join our upcoming meetup! Connect with fellow developers over coffee, share experiences, and discuss the latest industry trends. Enhance your network and gain new insights. Don’t miss this opportunity to learn and connect!",
                            EventCategory = EventCategory.Meetup,
                            Address = new Address()
                            {
                                Street = "123 Elm St",
                                City = "Charleston",
                                State = "WV",
                                ZipCode = 25301,
                                Country = "USA"
                            }
                        },
                        new Event()
                        {
                            Title = "Meetup",
                            Image = "/images/stock.jpg",
                            Description = "Join our upcoming meetup! Connect with fellow developers over coffee, share experiences, and discuss the latest industry trends. Enhance your network and gain new insights. Don’t miss this opportunity to learn and connect!",
                            EventCategory = EventCategory.Meetup,
                            Address = new Address()
                            {
                                Street = "456 Oak St",
                                City = "Madison",
                                State = "WI",
                                ZipCode = 53703,
                                Country = "USA"
                            }
                        },
                        new Event()
                        {
                            Title = "Meetup",
                            Image = "/images/stock8.jpg",
                            Description = "Join our upcoming meetup! Connect with fellow developers over coffee, share experiences, and discuss the latest industry trends. Enhance your network and gain new insights. Don’t miss this opportunity to learn and connect!",
                            EventCategory = EventCategory.Meetup,
                            Address = new Address()
                            {
                                Street = "789 Pine St",
                                City = "Cheyenne",
                                State = "WY",
                                ZipCode = 82001,
                                Country = "USA"
                            }
                        }
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
