using System;
using System.Threading.Tasks;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;

namespace DefaultNamespace
{
   public class BankDemoAppR3Service
   {
        private readonly Web3 web3;

        public static string ABI = @"[{'constant':false,'inputs':[{'name':'withDrawnAmount','type':'uint256'}],'name':'withdrawAmount','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[],'name':'getBalance','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'val','type':'uint256'}],'name':'getAccountSummary','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[],'name':'kill','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_to','type':'address'},{'name':'amountToSend','type':'uint256'},{'name':'description','type':'string'}],'name':'transferFunds','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'name':'','type':'uint256'}],'name':'accountSummaryDetails','outputs':[{'name':'TransactionId','type':'uint256'},{'name':'transactionType','type':'uint8'},{'name':'Description','type':'string'},{'name':'Debit','type':'uint256'},{'name':'Credit','type':'uint256'},{'name':'ReferenceNo','type':'address'},{'name':'Balance','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'description','type':'string'}],'name':'depositFunds','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'constant':false,'inputs':[{'name':'withDrawnAmount','type':'uint256'},{'name':'description','type':'string'}],'name':'withdrawFunds','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'inputs':[],'payable':false,'stateMutability':'nonpayable','type':'constructor'},{'anonymous':false,'inputs':[{'indexed':false,'name':'_message','type':'string'},{'indexed':false,'name':'amount','type':'uint256'}],'name':'withdrawStatus','type':'event'},{'anonymous':false,'inputs':[{'indexed':false,'name':'_message','type':'string'},{'indexed':false,'name':'depositor','type':'address'},{'indexed':false,'name':'amountDeposited','type':'uint256'}],'name':'depositStatus','type':'event'},{'anonymous':false,'inputs':[{'indexed':false,'name':'_message','type':'string'},{'indexed':false,'name':'SentTo','type':'address'},{'indexed':false,'name':'amountSent','type':'uint256'}],'name':'transferStatus','type':'event'},{'anonymous':false,'inputs':[{'indexed':false,'name':'_notOwnerMessage','type':'string'}],'name':'notOwner','type':'event'},{'anonymous':false,'inputs':[{'indexed':false,'name':'message','type':'string'},{'indexed':false,'name':'amount','type':'uint256'}],'name':'getBalanceEvent','type':'event'},{'anonymous':false,'inputs':[{'indexed':false,'name':'message','type':'string'},{'indexed':false,'name':'amount','type':'uint256'},{'indexed':false,'name':'name','type':'string'}],'name':'printAccountDetails','type':'event'},{'anonymous':false,'inputs':[{'indexed':false,'name':'message','type':'string'}],'name':'Failure','type':'event'}]";

        public static string BYTE_CODE = "0x6060604052341561000f57600080fd5b60008054600160a060020a033316600160a060020a0319909116179055610c408061003b6000396000f30060606040526004361061008d5763ffffffff7c01000000000000000000000000000000000000000000000000000000006000350416630562b9f7811461009257806312065fe0146100aa5780631465cbc4146100bd57806341c0e1b5146100d357806358a6f9ba146100e65780639ec37ba71461014b578063a45ccbc614610232578063ffe4560d14610278575b600080fd5b341561009d57600080fd5b6100a86004356102ce565b005b34156100b557600080fd5b6100a8610462565b34156100c857600080fd5b6100a860043561052e565b34156100de57600080fd5b6100a861054a565b34156100f157600080fd5b6100a860048035600160a060020a03169060248035919060649060443590810190830135806020601f8201819004810201604051908101604052818152929190602084018383808284375094965061056f95505050505050565b341561015657600080fd5b610161600435610748565b6040518088815260200187600381111561017757fe5b60ff1681526040810186905260608101859052600160a060020a038416608082015260a0810183905260c08282038101602083019081528854600260018216156101000260001901909116049183018290529160e001908890801561021d5780601f106101f25761010080835404028352916020019161021d565b820191906000526020600020905b81548152906001019060200180831161020057829003601f168201915b50509850505050505050505060405180910390f35b6100a860046024813581810190830135806020601f820181900481020160405190810160405281815292919060208401838380828437509496506107a195505050505050565b341561028357600080fd5b6100a8600480359060446024803590810190830135806020601f820181900481020160405190810160405281815292919060208401838380828437509496506108d595505050505050565b60005433600160a060020a039081169116141561041d57600154811161038c576001805482900390557f6960b931710bb3f7152c4cd470ed853ed130e2f20694c1ca495e7d065099c70881604051602081019190915260408082526028818301527f416d6f756e742057697468647261776e2066726f6d20796f7572204163636f7560608301527f6e74206973202d3e000000000000000000000000000000000000000000000000608083015260a0909101905180910390a1610418565b7ffd5de27667e5407df5be0b6efb7f86ae8c40433040399a5e4d36a46ccc50638d6040516020808252603a908201527f596f7520646f6e7420686176652073756666696369656e742046756e647320696040808301919091527f6e20796f7572204163636f756e7420746f20576974684472617700000000000060608301526080909101905180910390a15b61045f565b600080516020610bd58339815191526040516020808252601590820152600080516020610bf58339815191526040808301919091526060909101905180910390a15b50565b60005433600160a060020a03908116911614156104ea577fcaa0df9c86158d02ceffa7876bcb6353394e85347c2771efb1430db560c988e0600154604051602081019190915260408082526014818301527f54686973206973207468652062616c616e63652d00000000000000000000000060608301526080909101905180910390a161052c565b600080516020610bd58339815191526040516020808252601590820152600080516020610bf58339815191526040808301919091526060909101905180910390a15b565b60005433600160a060020a039081169116141561041d5761045f565b60005433600160a060020a03908116911614156104ea57600054600160a060020a0316ff5b60005433600160a060020a039081169116141561070157600154821161067057600160a060020a03831682156108fc0283604051600060405180830381858888f1935050505015156105c057600080fd5b6001805483900390557f326c36f5f56b5aa69ce4404109cb4a683ab837873e8768c5adc3e3211f6e9ef28383604051600160a060020a03909216602083015260408083019190915260608083526024908301527f46756e6473205472616e7366657265642066726f6d20796f75204163636f756e60808301527f74202d3e0000000000000000000000000000000000000000000000000000000060a083015260c0909101905180910390a16106fc565b7ffd5de27667e5407df5be0b6efb7f86ae8c40433040399a5e4d36a46ccc50638d6040516020808252603a908201527f596f7520646f6e7420686176652073756666696369656e742046756e647320696040808301919091527f6e20796f7572204163636f756e7420746f205472616e7366657200000000000060608301526080909101905180910390a15b610743565b600080516020610bd58339815191526040516020808252601590820152600080516020610bf58339815191526040808301919091526060909101905180910390a15b505050565b600480548290811061075657fe5b6000918252602090912060079091020180546001820154600383015460048401546005850154600686015494965060ff909316946002019391929091600160a060020a039091169087565b60018054340181556004805460009283926107bd918301610a60565b91506004828154811015156107ce57fe5b600091825260209091206007909102018054600190810182558101805460ff19166002908117909155909150810183805161080d929160200190610a8c565b5060006003820155346004820181905560058201805473ffffffffffffffffffffffffffffffffffffffff191633600160a060020a0381169190911790915560015460068401557fb5d7d11d8c35ac21ba8b8e1f99fa738fa9a28f05e704b41a07ffc8d4c40e0f1891604051600160a060020a0390921660208084019190915260408084019290925260608084528301527f416d6f756e74206465706f736974656420696e746f20746865206163636f7574608083015260a0909101905180910390a1505050565b60008054819033600160a060020a0390811691161415610a1857600154841161098757600180548590038155600480549161091291908301610a60565b915060048281548110151561092357fe5b60009182526020909120600790910201805460019081018255808201805460ff19169091179055905060028101838051610961929160200190610a8c565b506003810184905560006004820155600180546006830155600280549091019055610a13565b7ffd5de27667e5407df5be0b6efb7f86ae8c40433040399a5e4d36a46ccc50638d6040516020808252603a908201527f596f7520646f6e7420686176652073756666696369656e742046756e647320696040808301919091527f6e20796f7572204163636f756e7420746f20576974684472617700000000000060608301526080909101905180910390a15b610a5a565b600080516020610bd58339815191526040516020808252601590820152600080516020610bf58339815191526040808301919091526060909101905180910390a15b50505050565b815481835581811511610743576007028160070283600052602060002091820191016107439190610b0a565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f10610acd57805160ff1916838001178555610afa565b82800160010185558215610afa579182015b82811115610afa578251825591602001919060010190610adf565b50610b06929150610b76565b5090565b610b7391905b80821115610b0657600080825560018201805460ff19169055610b366002830182610b90565b506000600382018190556004820181905560058201805473ffffffffffffffffffffffffffffffffffffffff191690556006820155600701610b10565b90565b610b7391905b80821115610b065760008155600101610b7c565b50805460018160011615610100020316600290046000825580601f10610bb6575061045f565b601f01602090049060005260206000209081019061045f9190610b76560062355e78cc29138e8811a9954fd4c6e05cd54248af8c70086cf52c350d637741796f7520617265206e6f7420746865206f776e65720000000000000000000000a165627a7a7230582085058f9a14fd62be2d3805be30a8f491d2a6ed441b6ffed7ac9e45d47f432a5d0029";

        public static Task<string> DeployContractAsync(Web3 web3, string addressFrom,  HexBigInteger gas = null, HexBigInteger valueAmount = null) 
        {
            return web3.Eth.DeployContract.SendRequestAsync(ABI, BYTE_CODE, addressFrom, gas, valueAmount );
        }

        private Contract contract;

        public BankDemoAppR3Service(Web3 web3, string address)
        {
            this.web3 = web3;
            this.contract = web3.Eth.GetContract(ABI, address);
        }

        public Function GetFunctionWithdrawAmount() {
            return contract.GetFunction("withdrawAmount");
        }
        public Function GetFunctionGetBalance() {
            return contract.GetFunction("getBalance");
        }
        public Function GetFunctionGetAccountSummary() {
            return contract.GetFunction("getAccountSummary");
        }
        public Function GetFunctionKill() {
            return contract.GetFunction("kill");
        }
        public Function GetFunctionTransferFunds() {
            return contract.GetFunction("transferFunds");
        }
        public Function GetFunctionAccountSummaryDetails() {
            return contract.GetFunction("accountSummaryDetails");
        }
        public Function GetFunctionDepositFunds() {
            return contract.GetFunction("depositFunds");
        }
        public Function GetFunctionWithdrawFunds() {
            return contract.GetFunction("withdrawFunds");
        }

        public Event GetEventWithdrawStatus() {
            return contract.GetEvent("withdrawStatus");
        }
        public Event GetEventDepositStatus() {
            return contract.GetEvent("depositStatus");
        }
        public Event GetEventTransferStatus() {
            return contract.GetEvent("transferStatus");
        }
        public Event GetEventNotOwner() {
            return contract.GetEvent("notOwner");
        }
        public Event GetEventGetBalanceEvent() {
            return contract.GetEvent("getBalanceEvent");
        }
        public Event GetEventPrintAccountDetails() {
            return contract.GetEvent("printAccountDetails");
        }
        public Event GetEventFailure() {
            return contract.GetEvent("Failure");
        }


        public Task<string> WithdrawAmountAsync(string addressFrom, BigInteger withDrawnAmount, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionWithdrawAmount();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, withDrawnAmount);
        }
        public Task<string> GetBalanceAsync(string addressFrom,  HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionGetBalance();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount);
        }
        public Task<string> GetAccountSummaryAsync(string addressFrom, BigInteger val, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionGetAccountSummary();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, val);
        }
        public Task<string> KillAsync(string addressFrom,  HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionKill();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount);
        }
        public Task<string> TransferFundsAsync(string addressFrom, string _to, BigInteger amountToSend, string description, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionTransferFunds();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _to, amountToSend, description);
        }
        public Task<string> DepositFundsAsync(string addressFrom, string description, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionDepositFunds();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, description);
        }
        public Task<string> WithdrawFundsAsync(string addressFrom, BigInteger withDrawnAmount, string description, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionWithdrawFunds();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, withDrawnAmount, description);
        }

        public Task<AccountSummaryDetailsDTO> AccountSummaryDetailsAsyncCall(BigInteger a) {
            var function = GetFunctionAccountSummaryDetails();
            return function.CallDeserializingToObjectAsync<AccountSummaryDetailsDTO>(a);
        }


    }

    [FunctionOutput]
    public class AccountSummaryDetailsDTO 
    {
        [Parameter("uint256", "TransactionId", 1)]
        public BigInteger TransactionId {get; set;}

        [Parameter("uint8", "transactionType", 2)]
        public byte TransactionType {get; set;}

        [Parameter("string", "Description", 3)]
        public string Description {get; set;}

        [Parameter("uint256", "Debit", 4)]
        public BigInteger Debit {get; set;}

        [Parameter("uint256", "Credit", 5)]
        public BigInteger Credit {get; set;}

        [Parameter("address", "ReferenceNo", 6)]
        public string ReferenceNo {get; set;}

        [Parameter("uint256", "Balance", 7)]
        public BigInteger Balance {get; set;}

    }


    public class WithdrawStatusEventDTO 
    {
        [Parameter("string", "_message", 1, false)]
        public string _message {get; set;}

        [Parameter("uint256", "amount", 2, false)]
        public BigInteger Amount {get; set;}

    }

    public class DepositStatusEventDTO 
    {
        [Parameter("string", "_message", 1, false)]
        public string _message {get; set;}

        [Parameter("address", "depositor", 2, false)]
        public string Depositor {get; set;}

        [Parameter("uint256", "amountDeposited", 3, false)]
        public BigInteger AmountDeposited {get; set;}

    }

    public class TransferStatusEventDTO 
    {
        [Parameter("string", "_message", 1, false)]
        public string _message {get; set;}

        [Parameter("address", "SentTo", 2, false)]
        public string SentTo {get; set;}

        [Parameter("uint256", "amountSent", 3, false)]
        public BigInteger AmountSent {get; set;}

    }

    public class GetBalanceEventEventDTO 
    {
        [Parameter("string", "message", 1, false)]
        public string Message {get; set;}

        [Parameter("uint256", "amount", 2, false)]
        public BigInteger Amount {get; set;}

    }

    public class PrintAccountDetailsEventDTO 
    {
        [Parameter("string", "message", 1, false)]
        public string Message {get; set;}

        [Parameter("uint256", "amount", 2, false)]
        public BigInteger Amount {get; set;}

        [Parameter("string", "name", 3, false)]
        public string Name {get; set;}

    }


}

