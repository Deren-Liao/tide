#include "stdafx.h"

#include <iostream>
#include <vector>
using namespace std;


bool gen(vector<int> A, int len, int tot, vector<int>&sols, int pos)
/* should return bool */
{
	if (tot == 0) {
		return true;
	}
	for (int i = pos; i < len; i++) {
		if (A[i] <= tot) {
			sols.push_back(i);
			if (gen(A, len, tot - A[i], sols, i + 1))
				return true;
			sols.pop_back();
		}
	}
	return false;
}

vector<int> findUTVids(vector<int> A, int tot) {
	/* better to use reference  vector<int> &A */
	vector<int> sols;
	int len = A.size();
	gen(A, len, tot, sols, 0);
	return sols;
}


int main() {
	// your code goes here

	vector<int> A = { 5, 7, 20, 3, 5 };
	auto r = findUTVids(A, 17);
	for (auto i : r) {
		cout << i << " ";
	}
	cout << endl;

	return 0;
}

