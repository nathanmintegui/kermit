<script lang="ts">
	interface IProps {
		items: Array<ILegenda>;
	}

	interface ILegenda {
		id: number;
		nome: string;
		cor: string;
		dias: Array<Date>;
	}

	let { items }: IProps = $props();
</script>

<div class="m-12 flex-1 border">
	<p class="border-b bg-black text-white">Módulo</p>
	{#each items as item (item.id)}
		<div class="flex border-b">
			<span style="background-color: {item.cor};" class="w-[15%]">-</span>
			<p class="w-[85%] pl-4 text-start">{item.nome}</p>
		</div>
	{/each}
</div>
