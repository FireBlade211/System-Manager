using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireBlade.SystemManager.Plugin
{
    /// <summary>
    /// Represents the base interface for contracts.
    /// </summary>
    public interface IContract
    {
        /// <summary>
        /// Called whenever System Manager requests a contract implemented by this contract.
        /// </summary>
        /// <param name="contractIdentifier">The contract identifier. For contract identifers, see the <see cref="ContractIdentifiers"/>
        /// class.</param>
        /// <returns>The <see cref="IContract"/> for the specified identifier if this contract implements the contract;
        /// otherwise, <see langword="null"/>.</returns>
        IContract QueryContract(string contractIdentifier);
    }
}
