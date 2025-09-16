using System;
using System.Collections.Generic;
using System.Linq;

public class Solution
{
    public IList<IList<int>> FindCriticalAndPseudoCriticalEdges(int n, int[][] edges)
    {
        // Adiciona o índice original a cada aresta para fácil referência posterior.
        var edgesWithIndex = new List<int[]>();
        for (int i = 0; i < edges.Length; i++)
        {
            edgesWithIndex.Add(new int[] { edges[i][0], edges[i][1], edges[i][2], i });
        }

        // 1. Calcula o peso da MST do grafo original como referência.
        int originalMstWeight = Prim(n, edgesWithIndex, -1, -1);

        var criticalEdges = new List<int>();
        var pseudoCriticalEdges = new List<int>();

        // Itera sobre cada aresta para determinar seu tipo.
        for (int i = 0; i < edgesWithIndex.Count; i++)
        {
            // 2. Verifica se a aresta é CRÍTICA.
            // Calcula o peso da MST ignorando a aresta atual.
            int weightWithoutEdge = Prim(n, edgesWithIndex, i, -1);

            // Se o peso aumenta ou o grafo se desconecta (retorna infinito), a aresta é crítica.
            if (weightWithoutEdge > originalMstWeight)
            {
                criticalEdges.Add(edgesWithIndex[i][3]); // Adiciona o índice original
            }
            else
            {
                // 3. Se não for crítica, verifica se é PSEUDO-CRÍTICA.
                // Força a inclusão da aresta atual e calcula o peso da MST.
                int weightWithForcedEdge = Prim(n, edgesWithIndex, -1, i);

                // Se o peso for o mesmo que o original, ela pode fazer parte de uma MST.
                if (weightWithForcedEdge == originalMstWeight)
                {
                    pseudoCriticalEdges.Add(edgesWithIndex[i][3]); // Adiciona o índice original
                }
            }
        }

        return new List<IList<int>> { criticalEdges, pseudoCriticalEdges };
    }
    private int Prim(int n, List<int[]> edges, int ignoreEdgeIndex, int forceEdgeIndex)
    {
        // Cria a lista de adjacência para representar o grafo.
        var adj = new List<Tuple<int, int>>[n];
        for (int i = 0; i < n; i++)
        {
            adj[i] = new List<Tuple<int, int>>();
        }

        // Popula a lista de adjacência, ignorando a aresta especificada se houver.
        for (int i = 0; i < edges.Count; i++)
        {
            if (i == ignoreEdgeIndex) continue;
            int u = edges[i][0];
            int v = edges[i][1];
            int weight = edges[i][2];
            adj[u].Add(new Tuple<int, int>(v, weight));
            adj[v].Add(new Tuple<int, int>(u, weight));
        }

        // Fila de prioridade para o algoritmo de Prim: armazena <peso, vértice_destino>
        var pq = new PriorityQueue<Tuple<int, int>, int>();
        var visited = new bool[n];
        int totalWeight = 0;
        int edgeCount = 0;

        // Se uma aresta for forçada, inicializa a MST com ela.
        if (forceEdgeIndex != -1)
        {
            int[] forcedEdge = edges[forceEdgeIndex];
            int u = forcedEdge[0];
            int v = forcedEdge[1];
            int weight = forcedEdge[2];

            totalWeight += weight;
            edgeCount = 1; // Já temos uma aresta na MST.
            visited[u] = visited[v] = true;

            // Adiciona todas as arestas conectadas aos dois vértices da aresta forçada.
            foreach (var neighbor in adj[u])
            {
                if (!visited[neighbor.Item1])
                {
                    pq.Enqueue(new Tuple<int, int>(neighbor.Item1, neighbor.Item2), neighbor.Item2);
                }
            }
            foreach (var neighbor in adj[v])
            {
                if (!visited[neighbor.Item1])
                {
                    pq.Enqueue(new Tuple<int, int>(neighbor.Item1, neighbor.Item2), neighbor.Item2);
                }
            }
        }
        else
        {
            // Início padrão do Prim a partir do vértice 0.
            if (n > 0)
            {
                visited[0] = true;
                foreach (var neighbor in adj[0])
                {
                    pq.Enqueue(new Tuple<int, int>(neighbor.Item1, neighbor.Item2), neighbor.Item2);
                }
            }
        }

        // Loop principal do algoritmo de Prim.
        while (pq.Count > 0 && edgeCount < n - 1)
        {
            // Pega a aresta de menor peso que conecta a MST a um vértice não visitado.
            var edge = pq.Dequeue();
            int toNode = edge.Item1;
            int weight = edge.Item2;

            if (visited[toNode])
            {
                continue; // Evita ciclos.
            }

            visited[toNode] = true;
            totalWeight += weight;
            edgeCount++;

            // Adiciona as arestas do novo vértice à fila de prioridade.
            foreach (var neighbor in adj[toNode])
            {
                if (!visited[neighbor.Item1])
                {
                    pq.Enqueue(new Tuple<int, int>(neighbor.Item1, neighbor.Item2), neighbor.Item2);
                }
            }
        }

        // Se a MST não conectou todos os vértices, o grafo é desconexo.
        int finalNodeCount = visited.Count(v => v);
        return (finalNodeCount == n) ? totalWeight : int.MaxValue;
    }
}

public class Program 
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Executando o teste para o Problema 1489...");
        
        var solution = new Solution();

        // --- Exemplo 1 ---
        Console.WriteLine("\n--- Testando Exemplo 1 ---");
        int n1 = 5;
        int[][] edges1 = new int[][] 
        {
            new int[] {0,1,1}, new int[] {1,2,1}, new int[] {2,3,2}, 
            new int[] {0,3,2}, new int[] {0,4,3}, new int[] {3,4,3}, new int[] {1,4,6}
        };

        var result1 = solution.FindCriticalAndPseudoCriticalEdges(n1, edges1);
        Console.WriteLine("Arestas Críticas: [" + string.Join(", ", result1[0]) + "]");
        Console.WriteLine("Arestas Pseudo-Críticas: [" + string.Join(", ", result1[1]) + "]");
        Console.WriteLine("Saída Esperada: [[0, 1], [2, 3, 4, 5]]");


        // --- Exemplo 2 ---
        Console.WriteLine("\n--- Testando Exemplo 2 ---");
        int n2 = 4;
        int[][] edges2 = new int[][] 
        {
            new int[] {0,1,1}, new int[] {1,2,1}, new int[] {2,3,1}, new int[] {0,3,1}
        };

        var result2 = solution.FindCriticalAndPseudoCriticalEdges(n2, edges2);
        Console.WriteLine("Arestas Críticas: [" + string.Join(", ", result2[0]) + "]");
        Console.WriteLine("Arestas Pseudo-Críticas: [" + string.Join(", ", result2[1]) + "]");
        Console.WriteLine("Saída Esperada: [[], [0, 1, 2, 3]]");
    }
}
