using EquipmentTrackerThesis.Database;


namespace EquipmentTrackerThesis.Data
{
    /// <summary>
    /// Checks if any user is logged in.
    /// </summary>
    public class SignInCheck
    {
        private DatabaseContext _dbcontext;
        private DatabaseHandler _databaseHandler;
        private RoleService roleService;
        public EmployeeModel? CurrentEmployee { get; set; }

        public SignInCheck(DatabaseContext dbcontext, DatabaseHandler databaseHandler)
        {
            _dbcontext = dbcontext;
            _databaseHandler = databaseHandler;
        }

        /// <summary>
        /// This method checks if a correct username and password have been entered. 
        /// </summary>
        public bool SignInAttempt(string username, string password)
        {
            string hashedpass = _dbcontext.HashPassword(password);
            CurrentEmployee = _dbcontext.SignedInEmployee(username, hashedpass, _databaseHandler);
            if (CurrentEmployee == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// This method returns the role of the currently logged in user.
        /// </summary>
        public string GetRole()
        {
            roleService = new RoleService(_databaseHandler);
            return roleService.GetRole(CurrentEmployee?.Username ?? "");
        }
    }
}
