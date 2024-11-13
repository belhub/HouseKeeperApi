using HouseKeeperApi.Entities;

namespace HouseKeeperApi
{
    public class HouseSeeder(HouseKeeperDbContext dbContext)
    {
        private readonly HouseKeeperDbContext _dbContext = dbContext;

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
                //if (!_dbContext.Users.Any())//jeśli jest pusta - wypełniamy tabele
                //{
                //    var users = GetUsers();
                //    _dbContext.Users.AddRange(users);
                //    _dbContext.SaveChanges();//entity powinien zapisać dane w kontekście bazy danych
                //}
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            Console.WriteLine("ROle Admin: " + RoleNameEnum.Admin.ToString());
            var roles = new List<Role>() {
                new Role()
                {
                   RoleName = RoleNameEnum.Tenant.ToString(),
                },
                new Role()
                {
                    RoleName = RoleNameEnum.Landlord.ToString()

                },
                new Role()
                {
                    RoleName = RoleNameEnum.Admin.ToString()

                },
            };
            return roles;
        }

        //private IEnumerable<User> GetUsers()
        //{/* 
        //public required string Name { get; set; }
        //public required string Surname { get; set; }
        //public required string Email { get; set; }
        //  required int Phone { get; set; }
        //public DateTime? DateOfBirth { get; set; }
        //public required string PasswordHash { get; set; }
        //public int RoleId { get; set; }
        //  */
        //    var users = new List<User>()
        //    {
        //        new User() {

        //        }
        //    };
        //    return users;
        //}

    }
}
