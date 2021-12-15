import json

class AdaptiveTable:
    """
    AdaptiveTable class
    """
    @staticmethod
    def create_column(column_name : str, items_list : list) -> str:
        """
        Create a column header
        """
        column_header = f''"{"'"type":"TextBlock","text":"' 
        column_header += column_name
        column_header += '","weight":"bolder"}'

        column_items = []
        column_items.append(column_header)
        
        for item in items_list:
            column_items.append(",")
            column_name = "{"
            column_name += '"type":"TextBlock","text":"'
            column_name += item
            column_name += '","separator":"true"'
            column_name += "}"
            column_items.append(column_name)            
        
        str = "".join(column_items)

        column_item = f'"type" : "Column","items":[' 
        column_item += str
        column_item += ']'
        
        return column_item
    
    @staticmethod
    def create_column_list(items_list : list) -> str:

        column_item=[]
        count = 1
        n = len(items_list)
        for columninfo in items_list:
            column = '{' 
            column += columninfo
            column += '}'
            column_item.append(column)
            if count < len(items_list):
                column_item.append(",")
                count += 1
                   

        str = "".join(column_item)
        columninfo = f'"type": "ColumnSet", "columns": ['
        columninfo += str
        columninfo += ']'
        return columninfo
    
    @staticmethod
    def prepare_json(column_list : list) -> str:
        """
        Prepare the adaptive card
        """
        body = '{' 
        body += "".join(column_list)
        body += '}'
        adaptive_card ='{"$schema": "http://adaptivecards.io/schemas/adaptive-card.json",'
        adaptive_card += f'"type": "AdaptiveCard", "version": "1.0", "body": [ ' 
        adaptive_card += body
        adaptive_card += ']}'
        jobject = json.loads(adaptive_card)
        return jobject