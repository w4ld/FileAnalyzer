#geoip reference
'''
USING THIS--
You may have to correct the current pygeoip library to update references to urlopen 
it is now part of urllib.request. 
Find any use of urllib.urlopen and change to urllib.request.urlopen 
Find your module by importing sys and printing sys.path
I had to do this as geoip seemed to install in more than one location and my changes were not reflected at runtime.

'''

import sys
import getopt
from geoip import geolite2
import urllib.request
import geoip
def main(argv): #handles file args accordingle
    usage_string= "geoip_script.py [options]"
    try:
        opts, args = getopt.getopt(argv,"ha:",["address=","help"])
    except getopt.GetoptError:
        print(usage_string)
        sys.exit(2)
    for opt, arg in opts:
        if opt in ("-h", "--help"):
            help_string =  "ppdeep_script\n  Usage:  "+ usage_string
            help_string += "\n  Options:\n    -a <address> Get location information"
            print(help_string)
            sys.exit(0)
        elif opt in ("-a", "--address"):
            print(f"Address: {arg}")
            check_address(arg)
            return
    else:
        print(usage_string)
        sys.exit(0)
    
def check_address(address):
    #geolite2.get_info()
    # print(geoip.IpInfo(DatabaseInfo().filename)
    #match = geolite2.lookup_mine()
    match = geolite2.lookup(str(address))
    # match= None
    if match is not None:
        print(f'\tContinent:   {match.continent}')
        print(f'\tCountry:     {match.country}' )
        print(f'\tTimezone:    {match.timezone}')
        print(f'\tCoordinates: {match.location}')
    else:
        print("No information on IPv4 address.")           


if __name__ == "__main__":
   main(sys.argv[1:])







''' Documentation
            lookup(ip_addr)
    Looks up the IP information in the database and returns a IPInfo. If it does not exist, None is returned. What IP addresses are supported is specific to the GeoIP provider.

    Return type:	IPInfo
    lookup_mine()
    Looks up the computer’s IP by asking a web service and then checks the database for a match.

    Return type:	IPInfo
    class geoip.IPInfo(ip, data)
    Provides information about the located IP as returned by Database.lookup().

    continent
    The continent as ISO code if available.

    country
    The country code as ISO code if available.

    get_info_dict()
    Returns the internal info dictionary. For a maxmind database this is the metadata dictionary.

    ip
    The IP that was looked up.

    location
    The location as (lat, long) tuple if available.

    subdivisions
    The subdivisions as a list of ISO codes as an immutable set.

    timezone
    The timezone if available as tzinfo name.

    to_dict()
    A dict representation of the available information. This is a dictionary with the same keys as the attributes of this object.

    class geoip.DatabaseInfo
    Provides information about the GeoIP database.

    date = None
    Optionally the build date of the database as datetime object.

    filename = None
    If available the filename which backs the database.

    internal_name = None
    Optionally the internal name of the database.

    provider = None
    Optionally the name of the database provider.
'''