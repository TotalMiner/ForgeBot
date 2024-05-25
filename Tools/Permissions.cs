using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeBot.Tools
{
    public class Permissions
    {
        public static List<User> Users = new();
        public Permissions()
        {
            Users.Add(new User(269626330755104769, Permission.SUDO));
        }

        [System.Serializable]
        public class User
        {
            public User(ulong id, Permission level)
            {
                this.ID = id;
                this.OPLevel = level;
            }
            public ulong ID;
            public Permission OPLevel;

        }
        public enum Permission
        {
            SUDO = -1,
            Developer = 0,
            Admin = 1,
            Reserved = 2,
            Editor = 3,
            Unused = 4

        }
    }
}
