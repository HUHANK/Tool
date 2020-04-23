# -*- coding: UTF-8 -*-

import codecs
from comn import *
import MySQLdb

db = MySQLdb.connect(host='127.0.0.1', port=3306, user='root', passwd='123456', db='test', charset='utf8')
cursor = db.cursor()

def paraCPackFile():
    f = codecs.open("data.txt", "r", "utf-8")
    for line in f.readlines():
        line = line.strip()
        if len(line) < 1:
            continue
        # print(line)
        arr = line.split(";")
        val1 = arr[0]
        comm = ""
        if len(arr) > 1:
            comm = arr[1].strip().strip("/").strip("*").strip()
            print comm

        arr = val1.split()
        val_type = ""
        val_name = ""
        val_len = 0
        if arr[0] == "char":
            val_type = arr[0]
            val_name = arr[1].split("[")[0]
            val_len = int(arr[1].split("[")[1].strip("]"))
        elif arr[0] == "unsigned" and arr[1] == "char":
            val_type = "unsigned char"
            val_name = arr[2].split("[")[0]
            val_len = int(arr[2].split("[")[1].strip("]"))
        elif arr[0] == "long":
            val_type = arr[0]
            val_name = arr[1]
            val_len = 4
        elif arr[0] == "double":
            val_type = arr[0]
            val_name = arr[1]
            val_len = 8

        if len(val_type.strip()) == 0:
            print ("-----------------------------------")
        else:
            print ("%s,%s,%s" % (val_type, val_name, val_len))
            sql = "INSERT INTO cpack(NAME, TYPE, LEN, NOTE) VALUES('%s', '%s', %s, '%s')"%(val_name, val_type, val_len, comm)
            cursor.execute(sql)
            db.commit()

        # if val1.startswith("char") or (val1.startswith("unsigned") and val1.__contains__("char")):
        #     print (val1)
        # else:
        #     pass
        #     # print (val1)

# paraCPackFile()

def readFile(filePath, code="utf-8"):
    f = codecs.open(filePath, "r", code)
    lines = f.readlines()
    f.close()
    return lines

def writeLine(filePath, line, code="utf-8", mode='a'):
    f = codecs.open(filePath, mode, code)
    f.write(line+"\n")
    f.close()


# writeLine("./tmp/test.txt", "TEST", "utf-8", "w")
# writeLine("./tmp/test.txt", "TEST", "utf-8", 'w')
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

def removeComment(lines):
    stack = Stack()
    i=-1;
    retLines = []
    while i < len(lines):
        i += 1
        line = lines[i]

        if stack.is_empty() and line.__contains__("//"):
            if line.startswith("//"):
                continue
            else:
                str = line[0:line.find("//")]
                retLines.append(str)
                continue
        if stack.is_empty() and line.__contains__("/*"):
            ret = findSymbols(line, "/* */")

    for line in lines:
        retLine = ""
        # line = line.strip()
        # print line
        if stack.is_empty() and line.__contains__("//"):
            retLine = line[0:line.find("//")]
        elif line.__contains__("/*") or line.__contains__("*/"):
            ret = findSymbols(line, "/* */")
            if len(ret) == 1:
                if ret[0] == 0:
                    stack.push()
        elif not stack.is_empty():
            continue
        elif stack.is_empty():
            retLine = line
        print retLine



def MainProcess(filePath):
    lines = readFile(filePath, "gbk")
    lines = removeComment(lines)

TXCODES = "100102"

for txcode in TXCODES.split():
    filePath = "./back2/txcode/p%s.sqc"%(txcode)
    print u"现在开始处理: %s" % (filePath)
    MainProcess(filePath)