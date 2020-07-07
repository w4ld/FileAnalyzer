import sys
#sys.path.append(r"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python37_64\lib")
import os
import yara
import getopt
from os.path import isfile


def main(argv): #handles file args accordingle
    usage_string= "yara_script.py [options]"
    scanfile = ""
    scanfolder = ""
    
    try:
        opts, args = getopt.getopt(argv,"hf:F:",["file=","folder=","help"])
    except getopt.GetoptError:
        print(usage_string)
        sys.exit(2)
    if(len(opts)==0):
        print(usage_string)
    for opt, arg in opts:
        if opt in ("-h", "--help"):
            help_string =  "yara_script\n  Usage:  "+ usage_string
            help_string += "\n  Options:\n\t-h,--help\thelp\n\t-f,--file\tfile to scan"
            print(help_string)
            #TODO build a nice help.
            sys.exit()
        elif opt in ("-f", "--file"):
            scanfile = arg
        elif opt in ("-F", "--folder"):
            scanfolder = arg
        
   
    if len(scanfile) > 0:
        scan_file(scanfile)
    if len(scanfolder) > 0:
        scan_folder(scanfolder)


def scan_file(filename): #method for if a file is subscribed
    print(f"\tScanning file: {filename}")
    yara_stuff(filename)

def scan_folder(folderpath): #method if a directory path is subscribed
    print(f"\tScanning directory: {folderpath}")

#TODO make a source folder and read all yar files into a list and queue them up for scanning.
def yara_stuff(filename):
    ###credit for the yara rule,  https://github.com/CofenseLabs/Coronavirus-Phishing-Yara-Rules/blob/master/CofenseIntel_CoronavirusPhishing_Indicators.yar
    fromCsharp =r"..\..\..\..\PythonScripts\CofenseIntel_CoronavirusPhishing_In.yar" #SAMPLE LIBRARY FROM COFENSE RELATED TO COVID STUFF
    fromPython =r"CofenseIntel_CoronavirusPhishing_In.yar" #SAMPLE LIBRARY FROM COFENSE RELATED TO COVID STUFF
    yara_cofense_rule = fromPython if fromPython in os.listdir() else fromCsharp
    
    yara_rules_list = list()
    yara_rules_list.append(yara_cofense_rule)
    matches_list = list()
    try:
        for rulefile in yara_rules_list:
            rules = yara.compile(filepath=rulefile)#compile can take a multitude of arguments.
            matches = rules.match(filename, timeout=60)
            matches_list.append(matches)
            #print(matches_list)
        if(len(matches_list[0])>0): #checks for empty matches
            print_yara_matches(matches_list)
        else:
            print("\tNo matches.")
    except:
        print("\tError scanning file.")

def print_yara_matches(match_list): #prints all string matches from the rule
    for match in match_list:
        print(f"\tRule: {match[0].rule}")  #were referencing the only rule we have in our .yar file
        print(f"\t  Tags: {match[0].tags}")
        print("\t  Strings:")
        for mstr in match[0].strings:
            print_matched_string(mstr)

def print_matched_string(mstr):     #prints a formatted string match
    #(<offset>, <string identifier>, <string data>)
    print(f"\t    Offset: {mstr[0]}, String Identifier: {mstr[1]}, Data: {mstr[2]}")
   

    

if __name__ == "__main__":
   main(sys.argv[1:])


   
'''
YARA Notes
            
dictionary
{
'tags': ['foo', 'bar'],
'matches': True,
'namespace': 'default',
'rule': 'my_rule',
'meta': {},
'strings': [(81L, '$a', 'abc'), (141L, '$b', 'def')]
}
'''

'''
TODO if time, write some yara rules for identifying malicious code to demo with own ruleset.


'''