import sys
#sys.path.append(r"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python37_64\lib")
import getopt
import time
def main(argv): #handles file args accordingle
    usage_string= "TimedProgram.py [options]"
    
    try:
        opts, args = getopt.getopt(argv,"t:",["time="])
    except getopt.GetoptError:
        print(usage_string)
        sys.exit(2)
    for opt, arg in opts:
        if opt in ("-t", "--time"):
            tim = int(arg)
            #call time function
            timeAndPrint(tim)
    else:
        print(usage_string)
def timeAndPrint(tim):
    for i in range(30):
        print(f"Counter:{i}\t\twith a sleep of {tim} seconds")
        time.sleep(tim)

if __name__ == "__main__":
   main(sys.argv[1:])
