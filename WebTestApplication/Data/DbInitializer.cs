using WebTestApplication.Models;

namespace WebTestApplication.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TestApplicationContext context)
        {
            if (context.Contacts.Any())
            {
                return;
            }

            context.Contacts.AddRange(new [] {
                new Contact {
                    Name = "Angelo Daniel Chioreanu",
                    BirthDate = new DateOnly(1971, 1, 1),
                    Emails = new List<Email> {
                        new Email
                        {
                            IsPrimary = true,
                            Address = "angelodanielchioreanu1@gmail.com"
                        },
                        new Email
                        {
                            IsPrimary = false,
                            Address = "angelodanielchioreanu2@gmail.com"
                        },
                        new Email
                        {
                            IsPrimary = false,
                            Address = "angelodanielchioreanu3@gmail.com"
                        }
                    }
                },
                new Contact {
                    Name = "John Smith",
                    BirthDate = new DateOnly(1972, 2, 2),
                    Emails = new List<Email> {
                        new Email
                        {
                            IsPrimary = true,
                            Address = "johnsmith1@gmail.com"
                        },
                        new Email
                        {
                            IsPrimary = false,
                            Address = "johnsmith2@gmail.com"
                        }
                    }
                }
            });

            context.SaveChanges();
        }
    }
}
