import copy

print('hello world')

def subset(l, r, target):
    print l
    print r
    print target
    print ''
    i = 0
    if target == 0: return True
    for a in l:
        if a <= target:
            lcopy = l[:]
            del lcopy[i]
            rcopy = r[:]
            rcopy.append(a)
            if subset(lcopy, rcopy, target - a):
                del r[:]
                for x in rcopy: r.append(x)
                return True
        i += 1
    return False

l = [2, 3, 3, 7]
r = []
b = subset(l, r, 5)
print b
print r

