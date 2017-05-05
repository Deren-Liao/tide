  #!/usr/bin/env python
  import argparse

  class Credential(object):
      def get(self):
          # logic to get username/password from auth file
          pass
      def store(self):
          # logic to store username/password to auth file
          # its better to encrypt password if its in plain text
          pass
      def erase(self):
          # logic to delete auth file
          pass

  def main():
      parser = argparse.ArgumentParser()
      parser.add_argument('operation', action="store", type=str,
              help="Git action to be performed (get|store|erase)")
      # parser all arguments
      arguments = parser.parse_args()
      # get credentials
      credentials = Credential()

      if arguments.operation == "get":
          creds = credentials.get()
          print("username={0}".format(creds.get("username")))
          print("password={0}".format(creds.get("password")))
      elif arguments.operation == "store":
          credentials.store()
          # if credentials are already stored do not store again
      elif arguments.operation == "erase":
          credentials.erase()
      else:
          print "Invalid git operation"

  if __name__ == "__main__":
      main()