#include <bits/stdc++.h>

using namespace std;

class UFDS {
    private:
    vector<int> parent;
    vector<int> size;
    int qtdComponents;

    int component(int v) {
        if(v == parent[v]) return v;

        return parent[v] = component(parent[v]);
    }

    public:
    UFDS(int n): parent(n), size(n, 1), qtdComponents(n) {
        iota(parent.begin(), parent.end(), 0);
    }

    int getQtdComponents() {
        return qtdComponents;
    }

    bool join(int a, int b) {
        a = component(a);
        b = component(b);
        if(a == b) {
            return false;
        }

        if(size[b] > size[a]) {
            swap(a, b);
        }

        size[b] += size[a];
        parent[a] = b;
        qtdComponents--;
        return true;
    }

    bool isSameSet(int a, int b) {
        return component(a) == component(b);
    }
};

class Solution {
public:
    int maxStability(int n, vector<vector<int>>& edges, int k) {
        vector<tuple<int, int, int>> remainingEdges;

        int minUsedEdge = __INT_MAX__;
        UFDS ufds(n);
        for(auto &edge : edges) {
            if(edge[3]) {
                if(!ufds.join(edge[0], edge[1])) {
                    return -1;
                }
                minUsedEdge = min(minUsedEdge, edge[2]);
            } else {
                remainingEdges.emplace_back(edge[2], edge[0], edge[1]);
            }
        }

        sort(remainingEdges.rbegin(), remainingEdges.rend());

        stack<int> usedEdgesCost;
        for(auto &[s, u, v] : remainingEdges) {
            if(ufds.getQtdComponents() == 1) {
                break;
            }

            if(ufds.join(u, v)) {
                usedEdgesCost.emplace(s);
            }
        }
        if(ufds.getQtdComponents() != 1) {
            return -1;
        }

        int minRemainingCosts = minUsedEdge;
        while(!usedEdgesCost.empty()) {
            int s = usedEdgesCost.top();
            usedEdgesCost.pop();
            minRemainingCosts = min((k-- > 0)*s + s, minRemainingCosts);
        }

        return min(minRemainingCosts, minUsedEdge);
    }
};

int main() {
    auto solution = Solution();

    vector<vector<int>> case1 = {{0,1,2,1},{1,2,3,0}};
    assert(solution.maxStability(3, case1, 1) == 2);

    vector<vector<int>> case2 = {{0,1,4,0},{1,2,3,0},{0,2,1,0}};
    assert(solution.maxStability(3, case2, 2) == 6);

    vector<vector<int>> case3 = {{0,1,1,1},{1,2,1,1},{2,0,1,1}};
    assert(solution.maxStability(3, case3, 0) == -1);

    vector<vector<int>> case4 = {{0,1,87487,0}};
    assert(solution.maxStability(2, case4, 0) == 87487);
}
