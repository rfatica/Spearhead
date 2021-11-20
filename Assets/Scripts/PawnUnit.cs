using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnUnit : Unit
{
    public int Distance;
    
    public override int GetMoveCells(Vector3Int[] result, Gameboard board)
    {
        int count = 0;
        
        for (int y = -1; y <= 1; ++y)
        {
            for (int x = -1; x <= 1; ++x)
            {
                if(x == 0 && y == 0)
                    continue;
                
                //if(Mathf.Abs(x) + Mathf.Abs(y) > Distance)
                //    continue;
                count = CheckPhallanx(result, x, y, count);
                //var idx = m_CurrentCell + new Vector3Int(x, 0, y);
                //var unit = Gameboard.Instance.GetUnit(idx);
                //if (unit != null)
                //    continue;
                //if (board.IsOnBoard(idx))
                //{
                //    result[count] = idx;
                //    count++;
                //}
            }
        }

        return count;
    }

    private int CheckPhallanx(Vector3Int[] result, int dx, int dy, int count)
    {
        int phalLength = 1;
        int phalDist = 1;
        int enemyLength = 0;
        Vector3Int enemyPos = new Vector3Int(-1,-1,-1);
        bool enemyFound = false;
        bool enemyDone = false;
        for (int i = 1; i < 12; i++)
        {
            int x = dx * i;
            int y = dy * i;

            var idx = m_CurrentCell + new Vector3Int(x, 0, y);
            var unit = Gameboard.Instance.GetUnit(idx);
            if (!Gameboard.Instance.IsOnBoard(idx))
            {
                break;
            }
            if (unit != null)
            {
                if (unit.Side.CompareTo(this.Side) == 0)
                {
                    if (!enemyFound)
                    {
                        phalDist++;
                        phalLength++;
                    }
                } 
                else
                {
                    if (!enemyDone)
                    {
                        enemyLength++;
                        if (!enemyFound)
                        {
                            enemyPos = idx;
                            enemyFound = true;
                        }
                        phalDist--;
                    }
                }
            }
            else
            {
                if (!enemyFound)
                {
                    result[count] = idx;
                    count++;
                    phalDist--;
                    if (phalDist <= 0)
                        break;
                }
                if (enemyFound)
                {
                    enemyDone = true;
                }
            }
        }
        if (enemyFound && (phalLength > enemyLength))
        {
            result[count] = enemyPos;
            count++;
        }


        return count;
    }
}
