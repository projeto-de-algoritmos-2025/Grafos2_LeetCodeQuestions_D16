#include <bits/stdc++.h>

using namespace std;

class Solution {
public:
    int networkDelayTime(vector<vector<int>>& times, int n, int k) {
        vector<vector<tuple<int, int>>> graph(n+1);
        for(auto edge : times) {
            int u = edge[0];
            int v = edge[1];
            int w = edge[2];
            graph[u].emplace_back(v, w);
        }

        priority_queue<tuple<int, int, int>, vector<tuple<int,int,int>>, greater<tuple<int, int, int>>> pq;
        vector<int> parent(n+1, -1);
        vector<int> dist(n+1, -1);

        pq.emplace(0, k, k);
        dist[k] = 0;

        while(!pq.empty()) {
            auto [w, u, v] = pq.top();
            pq.pop();
        
            for(auto &[k, d] : graph[v]) {
                if(dist[k] == -1 || dist[v] + d < dist[k]) {
                    dist[k] = dist[v] + d;
                    pq.emplace(dist[v] + d, v, k);
                }
            }
        }

        int res = 0;
        for(int i = 1; i <= n; i++) {
            if(dist[i] == -1) {
                return -1;
            }
            res = max(res, dist[i]);
        }

        return res;
    }
};

int main() {
    auto s = Solution();

    vector<vector<int>> case1 = {{2,1,1},{2,3,1},{3,4,1}};
    assert(s.networkDelayTime(case1, 4, 2) == 2);

    vector<vector<int>> case2 = {{1,2,1}};
    assert(s.networkDelayTime(case2, 2, 1) == 1);

    vector<vector<int>> case3 = {{1,2,1}};
    assert(s.networkDelayTime(case3, 2, 2) == -1);
}