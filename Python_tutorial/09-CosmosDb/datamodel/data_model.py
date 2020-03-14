from enum import Enum
from botbuilder.core import StoreItem

class EnumUser(Enum):    
    NAME=1
    EMAIL=2
    PHONE=3
    DONE=4
    DB=5

class ConState:
    def __init__(self):
        self.profile = EnumUser.NAME
    @property
    def CurrentPos(self):
        return self.profile
    @CurrentPos.setter
    def EnumUser(self,current:EnumUser):
        self.profile = current

class UserProfile(StoreItem):
    def __init__(self):
        self.name = ""
        self.email=""
        self.phone=""

    @property
    def Name(self):
        return self.name
    @Name.setter
    def Name(self,name:str):
        self.name = name
    
    @property
    def Email(self):
        return self.email
    @Email.setter
    def Email(self,email:str):
        self.email = email

    @property
    def Phone(self):
        return self.phone
    @Phone.setter
    def Phone(self,phone:str):
        self.phone = phone