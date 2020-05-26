using UnityEngine;

public class Node
{
    
    public bool m_CanWalk;          //  标记结点是否可以通过
    public Vector3 m_WorldPos;    //  结点空间位置
    public int m_GridX;                  //  结点在数组的位置 X
    public int m_GridY;                  //  结点在数组的位置 X
    
    public int m_gCost;                 //  开始结点到当前结点的距离估值  g(x)
    public int m_hCost;                 //  当前结点到目标结点的距离估值  h(x)
    public int FCost                       //  总 Cost
    {
        get { return m_gCost + m_hCost; }
    }
    
    public Node m_Parent;           //  当前节点的父节点

    public Node(bool canWalk, Vector3 position, int gridX, int gridY)
    {
        m_CanWalk = canWalk;
        m_WorldPos = position;
        m_GridX = gridX;
        m_GridY = gridY;
    }
}

