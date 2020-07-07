
import wget

def prototype_download_all_script():
    url = r'https://www.filesignatures.net/index.php?page=all&currentpage='

    url_end = r'&order=DESCRIPTION&alpha=All'
    file_list = list()
    for i in range(1,19):
        file_list.append(wget.download(url+str(i)+url_end))
    return file_list


#  urllib.request.urlopen or urllib.request.Request 

def read_php_file(filename):
    # split_statement = r'class="login" src="images/img_off.png"'
    split_statement = r'<table id="innerTable" cellpadding="5" cellspacing="0">'
    ret_str=""
    f = open(filename, 'r')
    str_file = f.read()
    #remove some php code
    str_file = str_file.replace('<td bgcolor = #d0cece width="147">', '')
    str_file = str_file.replace('<td bgcolor = #d0cece width="236">', '')
    str_file = str_file.replace('<td bgcolor = #d0cece width="274">', '')
    str_file = str_file.replace('<td bgcolor = #f2eded width="147">', '')
    str_file = str_file.replace('<td bgcolor = #f2eded width="236">', '')
    str_file = str_file.replace('<td bgcolor = #f2eded width="274">', '')
    str_file =  str_file.replace(r'<span id = "results"><a href="/index.php?page=search&search=','')
    str_file = str_file.replace('<tr>','')
    #cut with a split, take latter half
    php_pieces = str_file.split(split_statement)[1]
    #cut with a split again into pieces
    php_pieces = php_pieces.split(r'</tr>')

    count=0
    #print(php_pieces)
    for piece in php_pieces:
        #now have rough magic header entries
        nu_piece = piece.split(r'class="login" src="images/img_off.png"')
        if len(nu_piece)>1:
            #remove more php
            nu_piece = nu_piece[1]
            nu_piece = nu_piece.split('EXT">')[1]
            nu_piece = nu_piece.replace('</a></span></td>','')
            nu_piece = nu_piece.replace('</td>','')
            #print(f'{count}\t{nu_piece}') ##to check what was going on.
            #finally have each on their own line
            nu_piece = nu_piece.split('\n') #split and do last cleaning
            nu_piece[1] = nu_piece[1].split('SIG">')[1]
            #ret_str+=f'{count})\tExtension: {nu_piece[0]}\n\t\tMagicHeader: {nu_piece[1]}\n\t\tDescription: {nu_piece[2].strip()}')   #end result looks good!
            # output for CSV 
            ret_str+=f'{nu_piece[0]}, {nu_piece[1]}, {nu_piece[2].strip()}\n'    
            
        count+=1
    return ret_str

def main():
    #quickish hackey database download 
    #writing regexes seemed to take more time than it would take for me to write this.
    magic_str =""
    file_list = prototype_download_all_script()
    for f in file_list:
        print(f)
        magic_str+=read_php_file(f)
    try:
        outfile = open(r'C:\Users\Derek\Downloads\magic_file.csv','a')
        outfile.write(magic_str)
    except Exception as ex:
        print(ex)
    finally:
        outfile.close()
    #beautiful... I'm really enjoying python.
if __name__ == "__main__":
    main()