public class Solution
{
    // Representa o estado atual: cidade, custo acumulado e número de paradas feitas
    class State
    {
        public int City, Cost, Stops;
        public State(int city, int cost, int stops)
        {
            City = city; Cost = cost; Stops = stops;
        }
    }

    public int FindCheapestPrice(int n, int[][] flights, int src, int dst, int K)
    {
        // Lista de adjacência para representar o grafo: cada cidade aponta para destinos com seus respectivos preços
        var adj = new List<(int to, int price)>[n];
        for (int i = 0; i < n; i++) adj[i] = new List<(int, int)>();
        foreach (var f in flights) adj[f[0]].Add((f[1], f[2]));

        // Número máximo de paradas permitidas (K paradas = K+1 voos)
        int maxStops = K + 1;

        // Matriz de distâncias: dist[i][j] representa o menor custo para chegar à cidade i com j paradas
        var dist = new int[n][];
        for (int i = 0; i < n; i++)
        {
            dist[i] = new int[maxStops + 1];
            for (int j = 0; j <= maxStops; j++)
                dist[i][j] = int.MaxValue / 2; // Inicializa com um valor alto para evitar overflow
        }
        dist[src][0] = 0; // Custo para sair da cidade de origem com 0 paradas é zero

        // Fila de prioridade (min-heap) baseada no custo: semelhante ao Dijkstra
        var pq = new PriorityQueue<State, int>();
        pq.Enqueue(new State(src, 0, 0), 0);

        // Processa os estados na ordem de menor custo
        while (pq.Count > 0)
        {
            var s = pq.Dequeue();

            // Se chegou ao destino, retorna o custo
            if (s.City == dst) return s.Cost;

            // Se já atingiu o número máximo de paradas, não pode continuar
            if (s.Stops == maxStops) continue;

            // Explora os vizinhos da cidade atual
            foreach (var (to, price) in adj[s.City])
            {
                int nc = s.Cost + price; // Novo custo acumulado
                int ns = s.Stops + 1;    // Nova contagem de paradas

                // Se encontrou um caminho mais barato para a cidade 'to' com 'ns' paradas, atualiza
                if (nc < dist[to][ns])
                {
                    dist[to][ns] = nc;
                    pq.Enqueue(new State(to, nc, ns), nc); // Adiciona novo estado à fila
                }
            }
        }

        // Se não encontrou caminho viável dentro do limite de paradas, retorna -1
        return -1;
    }
}

class Program
{
    static void Main()
    {
        var sol = new Solution();

        // Exemplo 1
        int n1 = 4;
        int[][] flights1 = new int[][] {
            new int[]{0,1,100},
            new int[]{1,2,100},
            new int[]{2,0,100},
            new int[]{1,3,600},
            new int[]{2,3,200}
        };
        Console.WriteLine(sol.FindCheapestPrice(n1, flights1, 0, 3, 1));  // 700

        // Exemplo 2
        int n2 = 3;
        int[][] flights2 = new int[][] {
            new int[]{0,1,100},
            new int[]{1,2,100},
            new int[]{0,2,500}
        };
        Console.WriteLine(sol.FindCheapestPrice(n2, flights2, 0, 2, 1));  // 200

        // Exemplo 3
        Console.WriteLine(sol.FindCheapestPrice(n2, flights2, 0, 2, 0));  // 500
    }
}
