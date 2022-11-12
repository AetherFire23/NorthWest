using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Utils
{
    // Mes conventions :
    //
    // U = Unity
    // UGF = Unity 'GameObject' Fixed (ex. UFRoomTabBar) - L'object est fixe et présent initialiement dans la scène. Accédé à travers l'injection. Le supprimer pourrait causer des comportements étranges
    // UGI = Unity GameObject Instance (UGIPlayer) - l'objet est créé et supprimé at runtime. Ne requiert pas d'injection de dépendance.
    // UI = User Interface
    //
    // AS = AccessScript = Permet d'accéder à l'objet de jeu à travers 

    // Wrapper veut dire partial class
    // Shared veut dire que partagé avec webAPI, le reste unique à unity.
    //
}
