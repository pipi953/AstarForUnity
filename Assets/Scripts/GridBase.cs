using System.Collections.Generic;
using UnityEngine;

public class GridBase : MonoBehaviour
{
    private Node[,] m_Grid;             //  网格
    public Vector2 m_GridSize;       //  网格大小
    public float m_NodeRadius;      //  结点半径
    public LayerMask m_Layer;       //  标记所在层
    public Stack<Node> m_Path = new Stack<Node>();
    private float m_NodeDiameter;   //  节点直径
    private int m_GridCountX;           //  X 轴上的网格个数
    private int m_GridCountY;           //  Y 轴上的网格个数

    void Start()
    {
        m_NodeDiameter = m_NodeRadius * 2;                                                   //  计算节点直径
        m_GridCountX = Mathf.RoundToInt(m_GridSize.x / m_NodeDiameter);     //  计算 X 轴上的网格个数（四舍五入）
        m_GridCountY = Mathf.RoundToInt(m_GridSize.y / m_NodeDiameter);     //  计算 Y 轴上的网格个数（四舍五入）
        m_Grid = new Node[m_GridCountX, m_GridCountY];                                //  创建保存网格中结点的二维数组
        CreateGrid();                                                                                             //  生成由结点构成的网格
    }

    /// <summary>
    /// 创建格子
    /// </summary>
    private void CreateGrid()
    {
        Vector3 startPos = transform.position;
        startPos.x = startPos.x - m_GridSize.x / 2;     //  计算起始点的实际位置
        startPos.z = startPos.z - m_GridSize.y / 2;     //  计算起始点的实际位置
        for (int i = 0; i < m_GridCountX; i++)
        {
            for (int j = 0; j < m_GridCountY; j++)
            {
                Vector3 worldPos = startPos;
                worldPos.x = worldPos.x + i * m_NodeDiameter + m_NodeRadius;        //  起始点在世界坐标系中的位置  
                worldPos.z = worldPos.z + j * m_NodeDiameter + m_NodeRadius;        //  起始点在世界坐标系中的位置
                bool canWalk = !Physics.CheckSphere(worldPos, m_NodeRadius, m_Layer);   //  通过检测球体是否与物体发生碰撞来设置当前节点是否可以通过 [取相反值]，即：若发生碰撞，返回 true，则将该网格设置为不可通过 false
                m_Grid[i, j] = new Node(canWalk, worldPos, i, j);                                   //  将起点位置放入网格数组中
            }
        }
    }

    /// <summary>
    /// 通过空间位置获得对应的节点
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Node GetFromPosition(Vector3 pos)
    {
        float percentX = (pos.x + m_GridSize.x / 2) / m_GridSize.x;     //  X 轴的所在行
        float percentZ = (pos.z + m_GridSize.y / 2) / m_GridSize.y;     //  Y 轴的所在列
        percentX = Mathf.Clamp01(percentX);                                   //  物体边界的控制，限制 value 值必须在 min 和 max 之间
        percentZ = Mathf.Clamp01(percentZ);                                   //  物体边界的控制，限制 value 值必须在 min 和 max 之间
        int x = Mathf.RoundToInt((m_GridCountX - 1) * percentX);     //  四舍五入获取空间位置对应的 数组中的 节点 
        int z = Mathf.RoundToInt((m_GridCountY - 1) * percentZ);     //  四舍五入获取空间位置对应的 数组中的 节点 
        return m_Grid[x, z];
    }

    /// <summary>
    /// 获得当前节点的相邻节点
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public List<Node> GetNeighor(Node node)
    {
        List<Node> neighborList = new List<Node>();     //  保存当前顶点的 相邻顶点
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                int tempX = node.m_GridX + i;
                int tempY = node.m_GridY + j;
                if (tempX < m_GridCountX && tempX > 0 && tempY > 0 && tempY < m_GridCountY)
                {
                    neighborList.Add(m_Grid[tempX, tempY]);     //  将上下左右 符合条件的节点添加到 list 中
                }
            }
        }
        return neighborList;
    }

    private void OnDrawGizmos()     //  绘制
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(m_GridSize.x, 1, m_GridSize.y));
        if (m_Grid == null)
        {
            return;
        }
        foreach (var node in m_Grid)
        {
            Gizmos.color = node.m_CanWalk ? Color.white : Color.red;
            Gizmos.DrawCube(node.m_WorldPos, Vector3.one * (m_NodeDiameter - 0.1f));
        }
        if (m_Path != null)
        {
            foreach (var node in m_Path)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(node.m_WorldPos, Vector3.one * (m_NodeDiameter - 0.1f));
            }
        }
    }
}
