import os
import sys
import json

def genProtoMap(file,targetPath) :
    pbDict = {}
    jsonList = []
    for root, dirs, files in os.walk(file):

        # root 表示当前正在访问的文件夹路径
        # dirs 表示该文件夹下的子目录名list
        # files 表示该文件夹下的文件list

        # 遍历文件
        for f in files:
            file_path = os.path.join(root, f)
            file_ext = file_path.rsplit(".",maxsplit=1)
            if len(file_ext) != 2:
                continue
            if file_ext[1] == "proto":
                with open(file_path,'rb') as f:
                    fileName =  os.path.basename(f.name)
                    fileName = fileName.split('.')[0]
                    for line in f:
                        # 去空格
                        line = line.strip()
                        if not line:
                            continue
                        # 获取协议id
                        if line.startswith(b'//GenProtoMap ID = '):
                            s=line.decode()
                            id = s.replace('//GenProtoMap ID = ','')
                            if pbDict.__contains__(id) :
                                print("err 重复定义协议 "+pbDict[id])
                                continue
                            nextLine = next(f)
                            if nextLine :
                                msgName = nextLine.decode()
                                msgName = msgName.replace("message",'')
                                msgName = msgName.replace("{",'').strip()
                                pbDict[id] = msgName
                                item = {}
                                item["msgID"] = id
                                item["msgName"] = msgName
                                item["pkgName"] = fileName
                                jsonList.append(item)
                    f.close()

        str_json = json.dumps(jsonList)
        writGenProtoMapFile(str_json,targetPath)
        # 遍历所有的文件夹 暂时不做处理
        # for d in dirs:
        #     print(os.path.join(root, d))

def writGenProtoMapFile(text,targetPath) :
    path = targetPath
    f = open(path,'w')
    f.write(text)
    f.close()
if __name__ == '__main__':
    # genProtoMap("./","./protoMap.json")
    sourPath = "./"
    targetPath = "./protoMap.json"
    if len(sys.argv)>=2 :
        sourPath = sys.argv[1]
    if len(sys.argv)>=3 :
        targetPath = sys.argv[2]
    genProtoMap(sourPath,targetPath)