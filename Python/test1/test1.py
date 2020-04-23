# -*- coding: UTF-8 -*-





str = "fasd/*fkjasdkfjasdkf*/jaksdjfklas"

def findSymbols(str, syms):
    if len(str) < 1 or len(syms) < 1:
        return []
    arr = syms.split()
    ret = []
    for i in range(len(str)):
        for sym in arr:
            if str[i:i+len(sym)] == sym:
                ret.append(i)
    return ret


ret = findSymbols(str, "/* */")


print ret
for r in ret:
    print r