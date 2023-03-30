float GetMessageParameter(int specifiedParameter, String message){


  int messageLength = message.length();

  String currentSubMessage = "";
  int parameterNumber = 0;
  bool firstParameter = true;

        
  for (int i = 0; i < messageLength; i++){
    if(message.charAt(i) == ','){
      if(firstParameter){
        firstParameter = false;
        currentSubMessage = "";
      }
      else{
        if(parameterNumber == specifiedParameter) return currentSubMessage.toFloat();
        currentSubMessage = "";
        parameterNumber += 1;
        
      }
    }
    else{
      currentSubMessage += message.charAt(i);          
    }
    
  }
}
