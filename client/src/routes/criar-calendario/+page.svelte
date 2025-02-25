<script lang="ts">
	import type { PageProps } from './$types';
	import { enhance } from '$app/forms';

	let { data, form }: PageProps = $props();

	let trilhas = $state(data.trilhas.map(t => {
		return { id: t.id, nome: t.nome, checked: false };
	}));

	interface MesCompetencia {
		id: number;
		label: string;
		data: Data;
		checked: boolean;
	}

	interface Data {
		mes: number,
		ano: number
	}

	/*
	* TODO: criar função que gera esse array de competências de acordo com o mês atual e projeta
	*  12 meses a frente.
	*  */
	const competencias: MesCompetencia[] = [
		{ id: 1, label: 'Janeiro', data: { ano: 2025, mes: 1 }, checked: false },
		{ id: 2, label: 'Fevereiro', data: { ano: 2025, mes: 2 }, checked: false },
		{ id: 3, label: 'Março', data: { ano: 2025, mes: 3 }, checked: false },
		{ id: 4, label: 'Abril', data: { ano: 2025, mes: 4 }, checked: false },
		{ id: 5, label: 'Maio', data: { ano: 2025, mes: 5 }, checked: false },
		{ id: 6, label: 'Junho', data: { ano: 2025, mes: 6 }, checked: false },
		{ id: 7, label: 'Julho', data: { ano: 2025, mes: 7 }, checked: false },
		{ id: 8, label: 'Agosto', data: { ano: 2025, mes: 8 }, checked: false },
		{ id: 9, label: 'Setembro', data: { ano: 2025, mes: 9 }, checked: false },
		{ id: 10, label: 'Outubro', data: { ano: 2025, mes: 10 }, checked: false },
		{ id: 11, label: 'Novembro', data: { ano: 2025, mes: 11 }, checked: false },
		{ id: 12, label: 'Dezembro', data: { ano: 2025, mes: 12 }, checked: false }
	];

	const adicionarNovaTrilha = (): void => {
		const nome = prompt('Nome da trilha:');
		if (!nome) {
			throw new Error('Nome da trilha é obrigatório');
		}
		trilhas.push({ id: 0, nome: nome, checked: false });
	};
</script>

<div class="cursor-default border min-h-screen p-12 flex justify-center prevent-select poppins-regular">
	<form method="POST" use:enhance class="flex flex-col items-center">
		{#if form?.error}
			<p class="error">{form.error}</p>
		{/if}

		<h2 class="font-bold text-2xl my-3">Edição</h2>
		<div class="flex gap-2 text-center mb-3">
			{#each data?.edicoes as edicao}
				<label class="custom-radio w-24">
					<input type="radio" name="edicao" value={edicao.nome} required />
					<span class="radio-btn">{edicao.nome}</span>
				</label>
			{/each}
		</div>

		<h2 class="font-bold text-2xl my-3">Trilhas</h2>
		<div class="flex gap-2 text-center mb-3">
			{#each trilhas as trilha}
				<label class="custom-radio w-24">
					<input type="checkbox" name="trilhas" value={trilha.nome} bind:checked={trilha.checked} />
					<span class="radio-btn">{trilha.nome}</span>
				</label>
			{/each}
			<button class="w-12 cursor-pointer"
							onclick={adicionarNovaTrilha}>+
			</button>
		</div>

		{#if trilhas.find(t => t.checked)}
			<h2 class="font-bold text-2xl my-3">Competências</h2>
			<div class="flex gap-6 text-center mb-3">
				{#each trilhas as trilha}
					{#if trilha.checked}
						<div class="flex flex-col gap-3">
							<p class="font-bold">{trilha.nome}</p>
							<div class="grid grid-cols-2 gap-4 text-center mb-3 border rounded-sm p-4">
								{#each competencias as competencia}
									<label class="custom-radio">
										<input type="checkbox" name={`competencias-${trilha.nome}`} value={JSON.stringify(competencia.data)}
													 bind:checked={competencia.checked} />
										<span class="radio-btn">{competencia.label}</span>
									</label>
								{/each}
							</div>
						</div>
					{/if}
				{/each}
			</div>
		{/if}

		<button class="border rounded-sm p-3 bg-blue-300 cursor-pointer hover:opacity-85">Cadastrar</button>
	</form>
</div>

<style>
    .custom-radio input {
        display: none;
    }

    .radio-btn {
        display: flex;
        align-items: center;
        cursor: pointer;
        padding: 0.5em;
        border: 1px solid #ccc;
        border-radius: 4px;
        transition: background-color 0.2s;
    }

    .radio-btn:hover {
        background-color: #f0f0f0;
    }

    .custom-radio input:checked + .radio-btn {
        border: 2px solid #4CAF50;
    }

    .prevent-select {
        -webkit-user-select: none; /* Safari */
        -ms-user-select: none; /* IE 10 and IE 11 */
        user-select: none; /* Standard syntax */
    }
</style>
