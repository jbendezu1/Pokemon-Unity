using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemons;

    public List<Pokemon> Pokemons
    {
        get
        {
            return pokemons;
        }
    }

    private void Start()
    {
       foreach (var pokemon in pokemons)
        {
            pokemon.Init();
        }
    }
    public Pokemon GetHealthyPokemon()
    {
        return pokemons.Where(x => x.HP > 0).FirstOrDefault();
    }

    public void Update()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            foreach (Pokemon x in pokemons)
            {
                if (x.HP <= 0)
                {
                    pokemons.Remove(x);
                }
            }
        }
        
    }
}
