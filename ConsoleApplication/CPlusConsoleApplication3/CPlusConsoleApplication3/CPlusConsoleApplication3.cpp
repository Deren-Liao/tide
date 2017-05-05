// CPlusConsoleApplication3.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <map>
#include <stdio.h>
#include <list>

using namespace std;

class Clip
{
public:
	int x;
	int y;
};

void TestMap()
{
	map<int, Clip> m;
	m[5] = Clip();
	// printf("%d", m[5].at(0));
}

void LoopList(list<Clip>& clips,  list<Clip>::iterator it)
{
	if (it != clips.end())
	{
		printf("enter LoopList, %d\n", it->x);
		for (auto it2 = it; it2 != clips.end(); ++it2)
		{
			LoopList(clips, ++it2);
		}
	}
}

int main()
{
	list<Clip> clips;
	for (int i = 0; i < 5; ++i)
	{
		Clip p;
		p.x = i;
		clips.push_back(p);
	}

	LoopList(clips, clips.begin());

    return 0;
}

