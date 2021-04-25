import os
import json

def genProtoMap(file) :
    pbDict = {}
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
                                pbName = nextLine.decode()
                                pbName = pbName.replace("message",'')
                                pbName = pbName.replace("{",'').strip()
                                pbDict[id] = pbName
                    f.close()
        jsonList = []
        for _id,_name in pbDict.items() :
            item = {}
            item["msgID"] = _id
            item["msgName"] = _name
            jsonList.append(item)
        str_json = json.dumps(jsonList)
        writGenProtoMapFile(str_json)
        # 遍历所有的文件夹 暂时不做处理
        # for d in dirs:
        #     print(os.path.join(root, d))
def writGenProtoMapFile(text) :
    path = "./protoMap.json"
    f = open(path,'w')
    f.write(text)
    f.close()
if __name__ == '__main__':
    genProtoMap("./")