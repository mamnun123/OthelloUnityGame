using UnityEngine;
using VRSYS.Core.Networking;

public class UserRoleDetection : MonoBehaviour
{
   [UserRoleSelector] public UserRole desktopRole;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UserRole localUserRole = NetworkUser.LocalInstance.userRole.Value;

        if(localUserRole == desktopRole)
        {
            // do code for desktop user...
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
