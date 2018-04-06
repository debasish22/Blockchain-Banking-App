pragma solidity ^0.4.19;

contract BankDemoAppR3 {
    
    address owner;
    uint accountBalance;
    uint counter;
    event withdrawStatus (string _message,uint amount);
    event depositStatus(string _message,address depositor,uint amountDeposited);
    event transferStatus(string _message,address SentTo,uint amountSent);
    event notOwner(string _notOwnerMessage);
    event getBalanceEvent(string message,uint amount);
    event printAccountDetails(string message, uint amount, string name);
    event Failure(string message);
   // event ShowAccountSummary(AccountSummary accountSummaryData);
    
    enum TransactionType{Transfer,Withdrawl,Deposit,AccountClosed}
     
    struct AccountSummary {
        uint TransactionId;
        TransactionType transactionType;
        string Description;
        uint Debit;
        uint Credit;
        address ReferenceNo;
        uint Balance;
    }
  
    mapping(uint=>AccountSummary) detailsForEachAccount;
    
    AccountSummary[] public accountSummaryDetails;
    
    modifier onlyOwner(){
        if(msg.sender==owner)
        _;
        else{
            notOwner("you are not the owner");
        }
    }
    
    //This gets executed when contract is deployed
    function BankDemoAppR3(){
        owner=msg.sender;
    }
    
    function depositFunds(string description) payable public{
      
        accountBalance=accountBalance + msg.value;
           uint id=accountSummaryDetails.length++;
           AccountSummary a=accountSummaryDetails[id];
         // var  a= detailsForEachAccount[counter];
           a.TransactionId+=1;
           a.transactionType=TransactionType.Deposit;
           a.Description=description;
           a.Debit=0;
           a.Credit=msg.value;
           a.ReferenceNo= msg.sender;
           a.Balance=accountBalance;
         
        
        //raise event when funds deposited
        depositStatus("Amount deposited into the accout",msg.sender,msg.value);
    }
    
    function getBalance() onlyOwner public {
        getBalanceEvent("This is the balance-",accountBalance);
    }
    
    function withdrawFunds(uint withDrawnAmount,string description) onlyOwner {
        //raise event when funds withdrawn
        if(withDrawnAmount<= accountBalance ){
                accountBalance-=withDrawnAmount;
           uint id=accountSummaryDetails.length++;
           AccountSummary a=accountSummaryDetails[id];
           //var  a= detailsForEachAccount[counter];
           a.TransactionId+=1;
           a.transactionType=TransactionType.Withdrawl;
           a.Description=description;
           a.Debit=withDrawnAmount;
           a.Credit=0;
           //a.ReferenceNo="Self WithDrawl";
           a.Balance=accountBalance;
           counter++;
         //  updateStatus("Amount Withdrawn from your Account is ->",withDrawnAmount);
          
        }
       else{
            
             Failure("You dont have sufficient Funds in your Account to WithDraw");

        }
       
    }
    
    function transferFunds(address _to,uint amountToSend,string description) onlyOwner {
        
        if(amountToSend <=accountBalance){
            
            _to.transfer(amountToSend);
            
             accountBalance -= amountToSend;
           
      
         transferStatus("Funds Transfered from you Account ->",_to,amountToSend);
        }
        else{
            
            Failure("You dont have sufficient Funds in your Account to Transfer");
        }
    }
  
    
    function withdrawAmount(uint withDrawnAmount) onlyOwner {
        //raise event when funds withdrawn
        if(withDrawnAmount<= accountBalance ){ 
         accountBalance -= withDrawnAmount;
           withdrawStatus("Amount Withdrawn from your Account is ->",withDrawnAmount);
          
        }
       else{
            
             Failure("You dont have sufficient Funds in your Account to WithDraw");

        }
       
    }
    function getAccountSummary(uint val) onlyOwner {
      // ShowAccountSummary(accountSummaryDetails[val]);
     }
 
    function kill() onlyOwner{
        selfdestruct(owner);
    }
    
 
    
}