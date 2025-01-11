using DSharpPlus.SlashCommands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForgeBot.Tools
{
    public class Permissions
    {
        public static Dictionary<ulong, Permission> Users = new();
        public Permissions()
        {
            Users.Add(269626330755104769, Permission.SUDO);
        }

        static public Permission GetUserLevel(ulong id, Permission fallback = Permission.User)
        {
            if (Users.ContainsKey(id))
                return Users[id];
            return fallback;
        }

        static public void AddUser(ulong id, Permission permission = Permission.User)
        {
            if (Users.ContainsKey(id))
                Users[id] = permission;
            else
                Users.Add(id, permission);
        }

        public enum Permission
        {
            SUDO = -1,
            Developer = 0,
            Admin = 1,
            Reserved = 2,
            Editor = 3,
            Unused = 4,
            User = 9

        }

        public class UserPermissionLevel : SlashCheckBaseAttribute
        {
            public Permission RequiredPermission;
            /// <summary>
            /// Checks if the user has the required Operator Level
            /// </summary>
            /// <param name="requiredPermission">Minimum Permssion</param>
            public UserPermissionLevel(Permission requiredPermission)
            {
                RequiredPermission = requiredPermission;
            }

            public override async Task<bool> ExecuteChecksAsync(InteractionContext context)
            {
                if (GetUserLevel(context.User.Id) <= RequiredPermission)
                    return true;
                return false;
            }
        }
    }
}
