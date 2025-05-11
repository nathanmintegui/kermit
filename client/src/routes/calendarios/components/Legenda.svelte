<script lang="ts">
	interface IProps {
		items: Array<ILegenda>;
	}

	interface ILegenda {
		id: number;
		nome: string;
		cor: string;
		datas: Array<Date>;
	}

	let { items }: IProps = $props();
</script>

<div class="m-12 flex-1 border">
	<p class="border-b bg-black text-white">Módulo</p>
	{#each items as item (item.id)}
		<div class="flex border-b">
			<span style="background-color: {item.cor};" class="w-[15%]">-</span>
			<p class="w-[70%] pl-4 text-start">{item.nome}</p>
			<p class="w-[15%] border-l text-center">{item.datas.length}</p>
		</div>
	{/each}
</div>
