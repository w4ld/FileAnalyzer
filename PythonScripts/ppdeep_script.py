import sys
import ppdeep
import getopt
def main(argv): #handles file args accordingle
    usage_string= "ppdeep_script.py [options]"
    try:
        opts, args = getopt.getopt(argv,"hf:",["file=","help"])
    except getopt.GetoptError:
        print(usage_string)
        sys.exit(2)
    for opt, arg in opts:
        if opt in ("-h", "--help"):
            help_string =  "ppdeep_script\n  Usage:  "+ usage_string
            help_string += "\n  Options:\n    -f <filename> Calculate ppdeep hash"
            print(help_string)
            sys.exit(0)
        elif opt in ("-f", "--file"):
            ppdeep_stuff(arg)
            return
    else:
        print(usage_string)
        sys.exit(0)
    

def ppdeep_stuff(filename):
    ####PPDEEP###########
    file_hash = ppdeep.hash_from_file(filename)
    ##TODO check out ppdeep.compare() for comparing fuzzy hashes
    print(file_hash)
    ##ppdeep.hash_from_file
    ##lets have fun with compare
    # f1 = r"C:\Users\Derek\Downloads\SampleFileFolder\testyara1.txt"
    # f2 = r"C:\Users\Derek\Downloads\SampleFileFolder\testyara2.txt"
    # f3 = r"C:\Users\Derek\Downloads\SampleFileFolder\testyara3.txt"
    # f4 = r"C:\Users\Derek\Downloads\SampleFileFolder\Quiz-week06.docx"
    # f1h = ppdeep.hash_from_file(f1)
    # f2h = ppdeep.hash_from_file(f2)
    # f3h = ppdeep.hash_from_file(f3)
    # f4h = ppdeep.hash_from_file(f4)
    # print("testyara1")
    # print(f"ppdeep: {f1h}")
    # print("testyara2")
    # print(f"ppdeep: {f2h}")
    # print("testyara3")
    # print(f"ppdeep: {f3h}")
    # print("weekquiz06")
    # print(f"ppdeep: {f4h}")
    # val = ppdeep.compare(f1h, f2h) #very similar, a few extra characters
    # print(f"Are {val} similar!")
    # val = ppdeep.compare(f2h, f3h) #only a vew characters in different case
    # print(f"Just a few capitals difference. {val} similar!")
    # val = ppdeep.compare(f3h, f4h) #should be quite different
    # print(f"Quite different. {val} similar!")
    
if __name__ == "__main__":
   main(sys.argv[1:])
