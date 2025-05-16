using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TableCollisions : MonoBehaviour
{
    public IFinish finishPlate;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out IFinish plate))
        {
            if (plate.CanFinish)
            {
                if (finishPlate == null)
                {
                    plate.SetFinishOutline(true);
                    finishPlate = plate;
                }

            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.TryGetComponent(out IFinish plate))
        {
            plate.SetFinishOutline(false);
            if(plate == finishPlate)
                finishPlate = null;
        }
    }
}
