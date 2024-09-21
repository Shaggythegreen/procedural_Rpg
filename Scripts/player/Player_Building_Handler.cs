
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;


    public class Player_Building_Handler : MonoBehaviour
    {
    public Vector2 RegionEdge;
    public LayerMask LayerMask;
    public Tilemap FloorM, WallM, DoorM;
    public Building_Object Building;
    public Map_Region_Handler Region;
    private void Start()
    {
        
        
        
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mospos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mospos.z = 0;
            if (Is_Space_Availabile(mospos, Building))
            {
                Build_Building_At_Location(new Vector3Int(Mathf.FloorToInt(mospos.x), Mathf.FloorToInt(mospos.y), 0), Building);
            }

        }
        
    }
    public void Build_Building_At_Location(Vector3Int Position, Building_Object building)
    {


        int randomrand = Random.Range(1, building.Width - 1);

        for (int x = 0; x < building.Width; x++)
        {
            for (int y = 0; y < building.Height; y++)
            {
                if (x > 0 && y > 0 && x != building.Width && y != building.Height)
                {
                    FloorM.SetTile(new Vector3Int(Position.x + x, Position.y + y), building.floorTile);
                }
                if (x == 0 && y == 0)
                {
                    WallM.SetTile(new Vector3Int(Position.x + x, Position.y + y), building.WallBlc);
                }
                if (x == building.Width - 1 && y == 0)
                {
                    WallM.SetTile(new Vector3Int(Position.x + building.Width, Position.y + y), building.WallBrc);
                }
                if (x == building.Width - 1 && y == building.Height - 1)
                {
                    WallM.SetTile(new Vector3Int(Position.x + x + 1, Position.y + y), building.WallTrc);
                }
                if (x == 0 && y == building.Height - 1)
                {
                    WallM.SetTile(new Vector3Int(Position.x + x, Position.y + y), building.WallTlc);
                }

                if (x == 0 && y > 0)
                {
                    WallM.SetTile(new Vector3Int(Position.x + x, Position.y + y), building.WallLeft);
                }
                if (x == building.Width - 1 && y > 0 && y < building.Height - 1)
                {
                    WallM.SetTile(new Vector3Int(Position.x + x + 1, Position.y + y), building.WallRight);
                }
                if (x > 0 && y == 0)
                {
                    WallM.SetTile(new Vector3Int(Position.x + x, Position.y + y), building.WallCenter);
                }
                if (x >= 1 && y == building.Height - 1)
                {
                    WallM.SetTile(new Vector3Int(Position.x + x, Position.y + y), building.WallCenter);
                }


            }
        }
        WallM.SetTile(new Vector3Int(Position.x + randomrand, Position.y), null);
        DoorM.SetTile(new Vector3Int(Position.x + randomrand, Position.y), building.DoorTile);
    }

    public bool Is_Space_Availabile(Vector3 position,Building_Object building)
    {
        for (int x = 0; x < building.Width; x++)
        {


            for (int y = 0; y < building.Height; y++)
            {
                Vector3Int NewCheckPos = new Vector3Int(Mathf.FloorToInt(position.x) + x , Mathf.FloorToInt(position.y) + y,0);

                if(WallM.GetTile(NewCheckPos) != null)
                {
                    return false;
                }
                if (FloorM.GetTile(NewCheckPos) != null)
                {
                    return false;
                }

                if (DoorM.GetTile(NewCheckPos) != null)
                {
                    return false;
                }
            }


        }

        return true;
    }

}
